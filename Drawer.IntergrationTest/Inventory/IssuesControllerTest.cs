using Drawer.Application.Helpers;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.Inventory
{
    [Collection(ApiInstanceCollection.Default)]
    public class IssuesControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public IssuesControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> NewItem()
        {
            var request = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString()
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var itemId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return itemId;
        }

        async Task<long> NewLocation()
        {
            var request = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        async Task<decimal> GetInventoryItemQuantity(long itemId, long locationId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
              ApiRoutes.InventoryItems.Get + $"?ItemId={itemId}&LocationId={locationId}");
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var inventoryItemList = await responseMessage.Content.ReadFromJsonAsync<List<InventoryItemQueryModel>>() ?? default!;
            return inventoryItemList.FirstOrDefault()?.Quantity ?? 0;
        }

        async Task<long> NewReceipt(long itemId, long locationId, decimal quantity)
        {
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = DateTime.Now,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var receiptId = await responseMessage.Content.ReadFromJsonAsync<long>();
            return receiptId;
        }

        async Task<long> CreateIssue(DateTime issueDateTime, long itemId, long locationId, decimal quantity, string? buyer)
        {
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Buyer = buyer
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var issueId = await responseMessage.Content.ReadFromJsonAsync<long>();
            return issueId;
        }

        async Task<IssueQueryModel?> GetIssue(long issueId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var issue = await responseMessage.Content.ReadFromJsonAsync<IssueQueryModel?>();
            return issue;
        }

        [Fact]
        public async Task Add_Issue_Will_Return_Ok()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);

            // Act
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Buyer = buyer
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Add);
            request.Content = JsonContent.Create(requestContent);
            var response = await _client.SendAsyncWithMasterAuthentication(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var issueId = await response.Content.ReadFromJsonAsync<long>();
            issueId.Should().BeGreaterThan(0);

            var issue = await GetIssue(issueId) ?? default!;
            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTimeLocal.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Add_Issue_Will_Increase_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);
            var beforeQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Buyer = buyer
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            var afterQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Assert
            afterQuantity.Should().Be(beforeQuantity - quantity);
        }

        [Fact]
        public async Task BatchAdd_Issue_Will_Return_Ok()
        {
            // Arrange
            var issueListDto = new List<IssueCommandModel>()
            {
                new IssueCommandModel()
                {
                    IssueDateTimeLocal = DateTime.Now,
                    ItemId = await NewItem(),
                    LocationId = await NewLocation(),
                    Buyer = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    Quantity = Random.Shared.Next(1, 100)
                },
                new IssueCommandModel()
                {
                    IssueDateTimeLocal = DateTime.Now,
                    ItemId = await NewItem(),
                    LocationId = await NewLocation(),
                    Buyer = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    Quantity = Random.Shared.Next(1, 100)
                }
            };
            foreach(var issue in issueListDto)
            {
                await NewReceipt(issue.ItemId, issue.LocationId, issue.Quantity);
            }

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.BatchAdd);
            request.Content = JsonContent.Create(issueListDto);
            var response = await _client.SendAsyncWithMasterAuthentication(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var issueIdList = await response.Content.ReadFromJsonAsync<List<long>>() ?? default!;
            issueIdList.Should().NotBeNull();
            issueIdList.Count.Should().Be(issueListDto.Count);
        }

        [Fact]
        public async Task Update_Issue_Will_Return_Ok()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity, buyer);

            itemId = await NewItem();
            locationId = await NewLocation();
            quantity = 50;
            issueDateTime = DateTime.Now.AddSeconds(10);
            buyer = Guid.NewGuid().ToString();

            // 재고 보충
            await NewReceipt(itemId, locationId, quantity);

            // Act
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Buyer = buyer
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{issueId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var issue = await GetIssue(issueId) ?? null!;
            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTimeLocal.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Update_Issue_Same_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();
            var quantityBefore = 50;
            var quantityAfter = 80;

            await NewReceipt(itemId, locationId, quantityBefore + quantityAfter);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantityBefore, buyer);

            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantityAfter,
                Buyer = buyer
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{issueId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore + quantityBefore - quantityAfter);
        }

        [Fact]
        public async Task Update_Issue_Different_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId1 = await NewItem();
            var locationId1 = await NewLocation();
            var quantity1 = 30;

            var itemId2 = await NewItem();
            var locationId2 = await NewLocation();
            var quantity2 = 50;

            await NewReceipt(itemId1, locationId1, quantity1);
            await NewReceipt(itemId2, locationId2, quantity2);

            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();
            var issueId = await CreateIssue(issueDateTime, itemId1, locationId1, quantity1, buyer);

            var inventoryQuantity1Before = await GetInventoryItemQuantity(itemId1, locationId1);
            var inventoryQuantity2Before = await GetInventoryItemQuantity(itemId2, locationId2);

            // Act
            var requestContent = new IssueCommandModel()
            {
                IssueDateTimeLocal = issueDateTime,
                ItemId = itemId2,
                LocationId = locationId2,
                Quantity = quantity2,
                Buyer = buyer
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{issueId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var inventoryQuantity1After = await GetInventoryItemQuantity(itemId1, locationId1);
            inventoryQuantity1After.Should().Be(inventoryQuantity1Before + quantity1);
            var inventoryQuantity2After = await GetInventoryItemQuantity(itemId2, locationId2);
            inventoryQuantity2After.Should().Be(inventoryQuantity2Before - quantity2);
        }

        [Fact]
        public async Task Delete_Issue_Will_Return_Ok()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity, buyer);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Issues.Remove.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var issue = await getResponse.Content.ReadFromJsonAsync<IssueQueryModel?>();
            issue.Should().BeNull();
        }

        [Fact]
        public async Task Delete_Issue_Will_Decrease_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity, buyer);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Issues.Remove.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore + quantity);
        }

        [Fact]
        public async Task Get_Issue_Will_Return_Created_Issue()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await NewReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity, buyer);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var issue = await responseMessage.Content.ReadFromJsonAsync<IssueQueryModel>() ?? default!;
            issue.Should().NotBeNull();
            issue.Id.Should().Be(issueId);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTimeLocal.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Get_Issues_Will_Return_Created_Issues()
        {
            var issueDateTime = DateTime.Now;

            var itemId1 = await NewItem();
            var locationId1 = await NewLocation();
            var quantity1 = 30;
            var issueDateTime1 = issueDateTime;
            var buyer1 = Guid.NewGuid().ToString();

            await NewReceipt(itemId1, locationId1, quantity1);
            var issueId1 = await CreateIssue(issueDateTime1, itemId1, locationId1, quantity1, buyer1);

            var itemId2 = await NewItem();
            var locationId2 = await NewLocation();
            var quantity2 = 30;
            var issueDateTime2 = issueDateTime;
            var buyer2 = Guid.NewGuid().ToString();

            await NewReceipt(itemId2, locationId2, quantity2);
            var issueId2 = await CreateIssue(issueDateTime2, itemId2, locationId2, quantity2, buyer2);


            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.GetList
                .AddQuery("From", issueDateTime.Date.ToDateFormat())
                .AddQuery("To", issueDateTime.Date.ToDateFormat()));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var issueList = await responseMessage.Content.ReadFromJsonAsync<List<IssueQueryModel>>();
            issueList.Should().NotBeNull();
            issueList.Should().Contain(x =>
                x.Id == issueId1 &&
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1 &&
                x.IssueDateTimeLocal.IsCloseTo(issueDateTime1, TimeSpan.FromTicks(10)) &&
                x.Buyer == buyer1);
            issueList.Should().Contain(x =>
                x.Id == issueId2 &&
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2 &&
                x.IssueDateTimeLocal.IsCloseTo(issueDateTime2, TimeSpan.FromTicks(10)) &&
                x.Buyer == buyer2);


        }
    }
}
