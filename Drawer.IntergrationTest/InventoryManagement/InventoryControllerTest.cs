using Drawer.Contract;
using Drawer.Contract.BasicInfo;
using Drawer.Contract.InventoryManagement;
using Drawer.Contract.Items;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.InventoryManagement
{
    [Collection(ApiInstanceCollection.Default)]
    public class InventoryControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public InventoryControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
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
            var request = new CreateLocationRequest(null, Guid.NewGuid().ToString(), null);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Create);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var response = await ResponseMessage.Content.ReadFromJsonAsync<CreateLocationResponse>() ?? default!;
            return response.Id;
        }

        async Task<decimal> GetInventoryQuantity(long itemId, long locationId)
        {
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.Inventory.Get+ $"?ItemId={itemId}&LocationId={locationId}");
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            var response = await getResponseMessage.Content.ReadFromJsonAsync<GetInventoryResponse>() ?? default!;
            return response.InventoryDetails.FirstOrDefault()?.Quantity ?? 0M;
        }

        [Fact]
        public async Task Update_Inventory_Returns_Ok()
        {
            // Arrange
            var itemId = await CreateItem();
            var locationId = await CreateLocation();
            var quantityChange = 30;
            var oldQuantity = await GetInventoryQuantity(itemId, locationId);

            // Act
            var request = new UpdateInventoryRequest(itemId, locationId, quantityChange);
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Inventory.Update);
            requestMessage.Content = JsonContent.Create(request);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var newQuantity = await GetInventoryQuantity(itemId, locationId);
            newQuantity.Should().Be(oldQuantity + quantityChange);
        }

        [Fact]
        public async Task BatchUpdateInventory_Returns_Ok()
        {
            // Arrange
            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantityChange1 = 30;
            var oldQuantity1 = await GetInventoryQuantity(itemId1, quantityChange1);


            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantityChange2 = 40;
            var oldQuantity2 = await GetInventoryQuantity(itemId2, locationId2);


            // Act
            var request = new BatchUpdateInventoryRequest(
                new List<BatchUpdateInventoryRequest.InventoryChange>()
                {
                    new BatchUpdateInventoryRequest.InventoryChange(itemId1, locationId1, quantityChange1),
                    new BatchUpdateInventoryRequest.InventoryChange(itemId2, locationId2, quantityChange2),
                });
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Inventory.BatchUpdate);
            requestMessage.Content = JsonContent.Create(request);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var newQuantity1 = await GetInventoryQuantity(itemId1, locationId1);
            newQuantity1.Should().Be(oldQuantity1 + quantityChange1);
            var newQuantity2 = await GetInventoryQuantity(itemId2, locationId2);
            newQuantity2.Should().Be(oldQuantity2 + quantityChange2);
        }

        [Fact]
        public async Task GetInventory_Returns_Ok_With_Content()
        {
            // Arrange
            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 40;

            var updateRequest = new BatchUpdateInventoryRequest(
                new List<BatchUpdateInventoryRequest.InventoryChange>()
                {
                    new BatchUpdateInventoryRequest.InventoryChange(itemId1, locationId1, quantity1),
                    new BatchUpdateInventoryRequest.InventoryChange(itemId2, locationId2, quantity2),
                });
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Inventory.BatchUpdate);
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Inventory.Get);
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await getResponseMessage.Content.ReadFromJsonAsync<GetInventoryResponse>() ?? default!;
            response.Should().NotBeNull();
            response.InventoryDetails.Should().Contain(x =>
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1);
            response.InventoryDetails.Should().Contain(x =>
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2);
        }


    }
}

