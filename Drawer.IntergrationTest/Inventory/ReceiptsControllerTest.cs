﻿using Drawer.Application.Services.Inventory.CommandModels;
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
    public class ReceiptsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public ReceiptsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
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
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Add);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var itemId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return itemId;
        }

        async Task<long> NewRootLocationGroup()
        {
            var request = new LocationGroupAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var groupId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return groupId;
        }

        async Task<long> NewLocation()
        {
            var request = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                GroupId = await NewRootLocationGroup()
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(request);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        async Task<decimal> GetInventoryItemQuantity(long itemId, long locationId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
              ApiRoutes.InventoryItems.Get + $"?ItemId={itemId}&LocationId={locationId}");
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var itemList = await responseMessage.Content.ReadFromJsonAsync<List<InventoryItemQueryModel>>() ?? default!;
            return itemList.FirstOrDefault()?.Quantity ?? 0;
        }

        async Task<long> NewReceipt(DateTime receiptDateTime, long itemId, long locationId, decimal quantity, string? seller)
        {
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Seller = seller
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var receiptId = await responseMessage.Content.ReadFromJsonAsync<long>();
            return receiptId;
        }

        async Task<ReceiptQueryModel?> GetReceipt(long receiptId)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var receipt = await responseMessage.Content.ReadFromJsonAsync<ReceiptQueryModel?>();
            return receipt;
        }

        [Fact]
        public async Task Add_Receipt_Will_Return_Ok()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Seller = seller
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var receiptId = await responseMessage.Content.ReadFromJsonAsync<long>();
            receiptId.Should().BeGreaterThan(0);

            var receipt = await GetReceipt(receiptId) ?? default!;
            receipt.Id.Should().Be(receiptId);
            receipt.ItemId.Should().Be(itemId);
            receipt.LocationId.Should().Be(locationId);
            receipt.Quantity.Should().Be(quantity);
            receipt.ReceiptDateTimeLocal.Should().BeCloseTo(receiptDateTime, TimeSpan.FromTicks(10));
            receipt.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Add_Receipt_Will_Increase_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var beforeQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Seller = seller
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            var afterQuantity = await GetInventoryItemQuantity(itemId, locationId);

            // Assert
            afterQuantity.Should().Be(beforeQuantity + quantity);
        }

        [Fact]
        public async Task BatchAdd_Receipt_Will_Return_Ok()
        {
            // Arrange
            var receiptListDto = new List<ReceiptCommandModel>()
            {
                new ReceiptCommandModel()
                {
                    ReceiptDateTimeLocal = DateTime.Now,
                    ItemId = await NewItem(),
                    LocationId = await NewLocation(),
                    Seller = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    Quantity = Random.Shared.Next(1, 100)
                },
                new ReceiptCommandModel()
                {
                    ReceiptDateTimeLocal = DateTime.Now,
                    ItemId = await NewItem(),
                    LocationId = await NewLocation(),
                    Seller = Guid.NewGuid().ToString(),
                    Note = Guid.NewGuid().ToString(),
                    Quantity = Random.Shared.Next(1, 100)
                }
            };


            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Receipts.BatchAdd);
            request.Content = JsonContent.Create(receiptListDto);
            var response = await _client.SendWithMasterAuthentication(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var receiptIdList = await response.Content.ReadFromJsonAsync<List<long>>() ?? default!;
            receiptIdList.Should().NotBeNull();
            receiptIdList.Count.Should().Be(receiptListDto.Count);
        }

        [Fact]
        public async Task Update_Receipt_Will_Return_Ok()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId, locationId, quantity, seller);

            itemId = await NewItem();
            locationId = await NewLocation();
            quantity = 50;
            receiptDateTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity,
                Seller = seller
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var receipt = await GetReceipt(receiptId) ?? default!;
            receipt.Should().NotBeNull();
            receipt.Id.Should().Be(receiptId);
            receipt.ItemId.Should().Be(itemId);
            receipt.LocationId.Should().Be(locationId);
            receipt.Quantity.Should().Be(quantity);
            receipt.ReceiptDateTimeLocal.Should().BeCloseTo(receiptDateTime, TimeSpan.FromTicks(10));
            receipt.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Update_Receipt_Same_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity1 = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId, locationId, quantity1, seller);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            var quantity2 = 50;
            receiptDateTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            // Act
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId,
                LocationId = locationId,
                Quantity = quantity2,
                Seller = seller
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore - quantity1 + quantity2);
        }

        [Fact]
        public async Task Update_Receipt_Different_ItemLocation_Will_Update_InventoryItem_Quantity()
        {
            // Arrange
            var itemId1 = await NewItem();
            var locationId1 = await NewLocation();
            var quantity1 = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId1, locationId1, quantity1, seller);
            var inventoryQuantity1Before = await GetInventoryItemQuantity(itemId1, locationId1);

            var itemId2 = await NewItem();
            var locationId2 = await NewLocation();
            var quantity2 = 50;
            receiptDateTime = DateTime.Now.AddSeconds(10);
            seller = Guid.NewGuid().ToString();

            var inventoryQuantity2Before = await GetInventoryItemQuantity(itemId2, locationId2);


            // Act
            var requestContent = new ReceiptCommandModel()
            {
                ReceiptDateTimeLocal = receiptDateTime,
                ItemId = itemId2,
                LocationId = locationId2,
                Quantity = quantity2,
                Seller = seller
            }; 
            var requestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{receiptId}"));
            requestMessage.Content = JsonContent.Create(requestContent);
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

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
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId, locationId, quantity, seller);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Receipts.Remove.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequestMessage);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var receipt = await getResponse.Content.ReadFromJsonAsync<ReceiptQueryModel?>();
            receipt.Should().BeNull();
        }

        [Fact]
        public async Task Delete_Receipt_Will_Decrease_InventoryItem_Quantity()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId, locationId, quantity, seller);
            var inventoryQuantityBefore = await GetInventoryItemQuantity(itemId, locationId);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Receipts.Remove.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            var inventoryQuantityAfter = await GetInventoryItemQuantity(itemId, locationId);
            inventoryQuantityAfter.Should().Be(inventoryQuantityBefore - quantity);
        }

        [Fact]
        public async Task Get_Receipt_Will_Return_Created_Receipt()
        {
            // Arrange
            var itemId = await NewItem();
            var locationId = await NewLocation();
            var quantity = 30;
            var receiptDateTime = DateTime.Now;
            var seller = Guid.NewGuid().ToString();

            var receiptId = await NewReceipt(receiptDateTime, itemId, locationId, quantity, seller);

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Receipts.Get.Replace("{id}", $"{receiptId}"));
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var receipt = await responseMessage.Content.ReadFromJsonAsync<ReceiptQueryModel>() ?? default!;
            receipt.Should().NotBeNull();
            receipt.Id.Should().Be(receiptId);
            receipt.ItemId.Should().Be(itemId);
            receipt.LocationId.Should().Be(locationId);
            receipt.Quantity.Should().Be(quantity);
            receipt.ReceiptDateTimeLocal.Should().BeCloseTo(receiptDateTime, TimeSpan.FromTicks(10));
            receipt.Seller.Should().Be(seller);
        }

        [Fact]
        public async Task Get_Receipts_Will_Return_Created_Receipts()
        {
            var receiptDateTime = DateTime.Now;

            var itemId1 = await NewItem();
            var locationId1 = await NewLocation();
            var quantity1 = 30;
            var receiptDateTime1 = receiptDateTime;
            var seller1 = Guid.NewGuid().ToString();

            var receiptId1 = await NewReceipt(receiptDateTime1, itemId1, locationId1, quantity1, seller1);

            var itemId2 = await NewItem();
            var locationId2 = await NewLocation();
            var quantity2 = 30;
            var receiptDateTime2 = receiptDateTime;
            var seller2 = Guid.NewGuid().ToString();

            var receiptId2 = await NewReceipt(receiptDateTime2, itemId2, locationId2, quantity2, seller2);


            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Receipts.GetList + $"?From={receiptDateTime.Date.ToDateFormat()}&To={receiptDateTime.Date.ToDateFormat()}");
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var receipts = await responseMessage.Content.ReadFromJsonAsync<List<ReceiptQueryModel>>() ?? default!;
            receipts.Should().NotBeNull();
            receipts.Should().Contain(x =>
                x.Id == receiptId1 &&
                x.ItemId == itemId1 &&
                x.LocationId == locationId1 &&
                x.Quantity == quantity1 &&
                x.ReceiptDateTimeLocal.IsCloseTo(receiptDateTime1, TimeSpan.FromTicks(10)) &&
                x.Seller == seller1);
            receipts.Should().Contain(x =>
                x.Id == receiptId2 &&
                x.ItemId == itemId2 &&
                x.LocationId == locationId2 &&
                x.Quantity == quantity2 &&
                x.ReceiptDateTimeLocal.IsCloseTo(receiptDateTime2, TimeSpan.FromTicks(10)) &&
                x.Seller == seller2);


        }
    }
}
