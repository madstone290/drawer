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
    public class ReceiptsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public ReceiptsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
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

        async Task<long> CreateReceipt(long itemId, long locationId, decimal quantity, DateTime receiptTime, string? seller)
        {
            var requestContent = new CreateReceiptRequest(itemId, locationId, quantity, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateReceiptResponse>() ?? default!;
            return response.Id;
        }

        async Task<GetReceiptResponse> GetReceipt(long receiptId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            return await responseMessage.Content.ReadFromJsonAsync<GetReceiptResponse>() ?? default!;
        }

        [Fact]
        public async Task Create_Receipt_Will_Return_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new CreateReceiptRequest(itemId, locationId, quantity, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateReceiptResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Id.Should().BeGreaterThan(0);

            var receipt = await GetReceipt(response.Id);
            receipt.Id.Should().Be(response.Id);
            receipt.ItemId.Should().Be(itemId);
            receipt.LocationId.Should().Be(locationId);
            receipt.Quantity.Should().Be(quantity);
            receipt.ReceiptTime.Should().BeCloseTo(receiptTime, TimeSpan.FromTicks(10));
            receipt.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Create_Receipt_Will_Increase_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var beforeQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new CreateReceiptRequest(itemId, locationId, quantity, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Create);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            var afterQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Assert
            afterQuantity.Should().Be(beforeQuantity + quantity);
        }

        [Fact]
        public async Task Update_Receipt_Will_Return_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await CreateReceipt(itemId, locationId, quantity, receiptTime, seller);

            itemId = await CreateItem();
            locationId = await CreateLocation();
            quantity = 50;
            receiptTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new UpdateReceiptRequest(itemId, locationId, quantity, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, 
                ApiRoutes.Receipts.Update.Replace("{id}",$"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var receipt = await GetReceipt(receiptId);
            receipt.Id.Should().Be(receiptId);
            receipt.ItemId.Should().Be(itemId);
            receipt.LocationId.Should().Be(locationId);
            receipt.Quantity.Should().Be(quantity);
            receipt.ReceiptTime.Should().BeCloseTo(receiptTime, TimeSpan.FromTicks(10));
            receipt.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Update_Receipt_Same_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity1 = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await CreateReceipt(itemId, locationId, quantity1, receiptTime, seller);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            var quantity2 = 50;
            receiptTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new UpdateReceiptRequest(itemId, locationId, quantity2, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore - quantity1 + quantity2);
        }

        [Fact]
        public async Task Update_Receipt_Different_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await CreateReceipt(itemId1, locationId1, quantity1, receiptTime, seller);
            var inventoryQuantity1Before = await GetInventoryItemQuantity(itemId1, locationId1);

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 50;
            receiptTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            var inventoryQuantity2Before = await GetInventoryItemQuantity(itemId2, locationId2);


            // Act
            var requestContent = new UpdateReceiptRequest(itemId2, locationId2, quantity2, receiptTime, seller);
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var inventoryQuantity1After = await GetInventoryItemQuantity(itemId1, locationId1);
            inventoryQuantity1After.Should().Be(inventoryQuantity1Before - quantity1);
            var inventoryQuantity2After = await GetInventoryItemQuantity(itemId2, locationId2);
            inventoryQuantity2After.Should().Be(inventoryQuantity2Before + quantity2);
        }

        [Fact]
        public async Task Delete_Receipt_Will_Return_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await CreateReceipt(itemId, locationId, quantity, receiptTime, seller);
            
            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Receipts.Delete.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Receipt_Will_Decrease_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await CreateReceipt(itemId, locationId, quantity, receiptTime, seller);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Receipts.Delete.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore - quantity);
        }

        [Fact]
        public async Task Get_Receipt_Will_Return_Created_Receipt()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantity = 30;
            var receiptTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();
            
            var receiptId = await CreateReceipt(itemId, locationId, quantity, receiptTime, seller);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<GetReceiptResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Id.Should().Be(receiptId);
            response.ItemId.Should().Be(itemId);
            response.LocationId.Should().Be(locationId);
            response.Quantity.Should().Be(quantity);
            response.ReceiptTime.Should().BeCloseTo(receiptTime, TimeSpan.FromTicks(10));
            response.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Get_Receipts_Will_Return_Created_Receipts()
        {
            var receiptTime = DateTime.Now;

            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;
            var receiptTime1 = receiptTime;
            var seller1 = Guid.NewGuid().ToString();

            var receiptId1 = await CreateReceipt(itemId1, locationId1, quantity1, receiptTime1, seller1);

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 30;
            var receiptTime2 = receiptTime;
            var seller2 = Guid.NewGuid().ToString();

            var receiptId2 = await CreateReceipt(itemId2, locationId2, quantity2, receiptTime2, seller2);


            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.Receipts.GetList +$"?From={receiptTime.Date.ToDateFormat()}&To={receiptTime.Date.ToDateFormat()}");
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<GetReceiptsResponse>() ?? default!;
            response.Should().NotBeNull();
            response.Receipts.Should().Contain(x=> 
                x.Id == receiptId1 &&
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1 &&
                x.ReceiptTime.IsCloseTo(receiptTime1, TimeSpan.FromTicks(10)) &&
                x.Seller == seller1);
            response.Receipts.Should().Contain(x =>
                x.Id == receiptId2 &&
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2 &&
                x.ReceiptTime.IsCloseTo(receiptTime2, TimeSpan.FromTicks(10)) &&
                x.Seller == seller2);


        }
    }
}
