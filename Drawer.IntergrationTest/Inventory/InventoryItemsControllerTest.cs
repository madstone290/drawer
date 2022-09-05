using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.Inventory
{
    [Collection(ApiInstanceCollection.Default)]
    public class InventoryItemsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public InventoryItemsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> CreateItem()
        {
            var request = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString()
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Add);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var itemId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return itemId;
        }

        async Task<long> CreateGroup()
        {
            var requestContent = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var groupId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return groupId;
        }

        async Task<long> CreateLocation()
        {
            var request = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                GroupId = await CreateGroup()
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        async Task<decimal> GetInventoryQuantity(long itemId, long locationId)
        {
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.InventoryItems.Get + $"?ItemId={itemId}&LocationId={locationId}");
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            var inventoryItemList = await getResponseMessage.Content.ReadFromJsonAsync<List<InventoryItemQueryModel>>() ?? default!;
            return inventoryItemList.FirstOrDefault()?.Quantity ?? 0M;
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
            var request = new InventoryItemCommandModel()
            {
                ItemId = itemId,
                LocationId = locationId,
                QuantityChange = quantityChange
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.InventoryItems.Update);
            requestMessage.Content = JsonContent.Create(request);
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var newQuantity = await GetInventoryQuantity(itemId, locationId);
            newQuantity.Should().Be(oldQuantity + quantityChange);
        }

        [Fact]
        public async Task GetInventory_Returns_Ok()
        {
            // Arrange
            var itemId1 = await CreateItem();
            var locationId1 = await CreateLocation();
            var quantity1 = 30;

            var itemId2 = await CreateItem();
            var locationId2 = await CreateLocation();
            var quantity2 = 40;

            var updateRequest = new List<InventoryItemCommandModel>()
            {
                new InventoryItemCommandModel()
                {
                    ItemId = itemId1,
                    LocationId = locationId1,
                    QuantityChange =quantity1
                },
                new InventoryItemCommandModel()
                {
                    ItemId = itemId2,
                    LocationId = locationId2,
                    QuantityChange =quantity2
                },
            };
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.InventoryItems.BatchUpdate);
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.InventoryItems.Get);
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var inventoryItemList = await getResponseMessage.Content.ReadFromJsonAsync<List<InventoryItemQueryModel>>() ?? default!;
            inventoryItemList.Should().NotBeNull();
            inventoryItemList.Should().Contain(x =>
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1);
            inventoryItemList.Should().Contain(x =>
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2);
        }


    }
}

