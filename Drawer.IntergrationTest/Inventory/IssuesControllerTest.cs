using Drawer.Application.Helpers;
using Drawer.Contract;
using Drawer.Contract.Inventory;
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

        async Task<long> CreateItem()
        {
            var request = new CreateItemRequest(Guid.NewGuid().ToString(), null, null, null, null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await ResponseMessage.Content.ReadFromJsonAsync<CreateItemResponse>() ?? default!;
            return response.Id;
        }

        async Task<long> CreateLocation()
        {
            var request = new CreateLocationRequest(null, Guid.NewGuid().ToString(), null, false);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await ResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? default!;
            return response.Id;
        }

        async Task<decimal> GetInventoryItemQuantity(long itemId, long locationId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
              ApiRoutes.InventoryItems.Get + $"?ItemId={itemId}&LocationId={locationId}");
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var responseContent = await responseMessage.Content.ReadFromJsonAsync<GetInventoryItemsResponse>();
            return responseContent!.InventoryItems.FirstOrDefault()?.Quantity ?? 0;
        }

        async Task<long> CreateReceipt(long itemId, long locationId, decimal quantity)
        {
            var requestContent = new CreateReceiptRequest(DateTime.Now, itemId, locationId, quantity, null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateReceiptResponse>() ?? default!;
            return response.Id;
        }

        async Task<long> CreateIssue(DateTime issueDateTime, long itemId, long locationId, decimal quantity, string? buyer)
        {
            var requestContent = new CreateIssueRequest(issueDateTime, itemId, locationId, quantity, buyer);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateIssueResponse>() ?? default!;
            return response.Id;
        }

        async Task<IssueContracts.Issue> GetIssue(long issueId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var responseContent = await responseMessage.Content.ReadFromJsonAsync<GetIssueResponse>() ?? default!;
            return responseContent.Issue;
        }

        [Fact]
        public async Task Create_Issue_Will_Return_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);

            // Act
            var requestContent = new CreateIssueRequest(issueDateTime, itemId, locationId, quantity,  buyer);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateIssueResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Id.Should().BeGreaterThan(0);

            var issue = await GetIssue(response.Id);
            issue.Id.Should().Be(response.Id);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTime.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Create_Issue_Will_Increase_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);
            var beforeQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new CreateIssueRequest(issueDateTime, itemId, locationId, quantity,  buyer);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Issues.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            var afterQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Assert
            afterQuantity.Should().Be(beforeQuantity - quantity);
        }

        [Fact]
        public async Task Update_Issue_Will_Return_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity,  buyer);

            itemId = await CreateItem();
            locationId = await CreateLocation();
            quantity = 50;
            issueDateTime = DateTime.Now.AddSeconds(10);
            buyer = Guid.NewGuid().ToString();

            // 재고 보충
            await CreateReceipt(itemId, locationId, quantity);

            // Act
            var requestContent = new UpdateIssueRequest(issueDateTime, itemId, locationId, quantity,  buyer);
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Issues.Update.Replace("{id}", $"{issueId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var issue = await GetIssue(issueId);
            issue.Id.Should().Be(issueId);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTime.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Update_Issue_Same_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();
            var quantityBefore = 50;
            var quantityAfter = 80;

            await CreateReceipt(itemId, locationId, quantityBefore + quantityAfter);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantityBefore, buyer);

            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new UpdateIssueRequest(issueDateTime, itemId, locationId, quantityAfter, buyer);
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
            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 50;

            await CreateReceipt(itemId1, locationId1, quantity1);
            await CreateReceipt(itemId2, locationId2, quantity2);

            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();
            var issueId = await CreateIssue(issueDateTime, itemId1, locationId1, quantity1, buyer);

            var inventoryQuantity1Before = await GetInventoryItemQuantity(itemId1, locationId1);
            var inventoryQuantity2Before = await GetInventoryItemQuantity(itemId2, locationId2);

            // Act
            var requestContent = new UpdateIssueRequest(issueDateTime, itemId2, locationId2, quantity2, buyer);
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
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity,  buyer);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Issues.Delete.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Issue_Will_Decrease_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity,  buyer);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Issues.Delete.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore + quantity);
        }

        [Fact]
        public async Task Get_Issue_Will_Return_Created_Issue()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var issueDateTime = DateTime.Now;
            var buyer = Guid.NewGuid().ToString();

            await CreateReceipt(itemId, locationId, quantity);
            var issueId = await CreateIssue(issueDateTime, itemId, locationId, quantity,  buyer);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.Get.Replace("{id}", $"{issueId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var responseContent = await responseMessage.Content.ReadFromJsonAsync<GetIssueResponse>() ?? default!;
            responseContent.Should().NotBeNull();
            var issue = responseContent.Issue;
            issue.Id.Should().Be(issueId);
            issue.ItemId.Should().Be(itemId);
            issue.LocationId.Should().Be(locationId);
            issue.Quantity.Should().Be(quantity);
            issue.IssueDateTime.Should().BeCloseTo(issueDateTime, TimeSpan.FromTicks(10));
            issue.Buyer.Should().Be(buyer);
        }

        [Fact]
        public async Task Get_Issues_Will_Return_Created_Issues()
        {
            var issueDateTime = DateTime.Now;

            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;
            var issueDateTime1 = issueDateTime;
            var buyer1 = Guid.NewGuid().ToString();

            await CreateReceipt(itemId1, locationId1, quantity1);
            var issueId1 = await CreateIssue(issueDateTime1, itemId1, locationId1, quantity1, buyer1);

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 30;
            var issueDateTime2 = issueDateTime;
            var buyer2 = Guid.NewGuid().ToString();

            await CreateReceipt(itemId2, locationId2, quantity2);
            var issueId2 = await CreateIssue(issueDateTime2, itemId2, locationId2, quantity2, buyer2);


            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Issues.GetList
                .AddQuery("From", issueDateTime.Date.ToDateFormat())
                .AddQuery("To", issueDateTime.Date.ToDateFormat()));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<GetIssuesResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Issues.Should().Contain(x =>
                x.Id == issueId1 &&
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1 &&
                x.IssueDateTime.IsCloseTo(issueDateTime1, TimeSpan.FromTicks(10)) &&
                x.Buyer == buyer1);
            response.Issues.Should().Contain(x =>
                x.Id == issueId2 &&
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2 &&
                x.IssueDateTime.IsCloseTo(issueDateTime2, TimeSpan.FromTicks(10)) &&
                x.Buyer == buyer2);


        }
    }
}
