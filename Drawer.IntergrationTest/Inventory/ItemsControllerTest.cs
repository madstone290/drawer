using Drawer.Application.Services.Inventory.CommandModels;
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
    public class ItemsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public ItemsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task CreateItem_Returns_Ok_With_Content()
        {
            // Arrange
            var itemDto = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            request.Content = JsonContent.Create(itemDto);

            // Act
            var response = await _client.SendAsyncWithMasterAuthentication(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var itemId = await response.Content.ReadFromJsonAsync<long>();
            itemId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task BatchCreateItem_Returns_Ok_With_Content()
        {
            // Arrange
            var itemListDto = new List<ItemCommandModel>()
            {
                new ItemCommandModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Code = Guid.NewGuid().ToString(),
                    Number = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    QuantityUnit = Guid.NewGuid().ToString(),
                },
                new ItemCommandModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Code = Guid.NewGuid().ToString(),
                    Number = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    QuantityUnit = Guid.NewGuid().ToString(),
                }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.BatchCreate);
            request.Content = JsonContent.Create(itemListDto);

            // Act
            var response = await _client.SendAsyncWithMasterAuthentication(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var idList = await response.Content.ReadFromJsonAsync<List<long>>() ?? default!;
            idList.Should().NotBeNull();
            idList.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetItem_Returns_Ok_With_CreatedItem()
        {
            // Arrange
            var itemDto = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequest.Content = JsonContent.Create(itemDto);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var itemId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", $"{itemId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var item = await getResponse.Content.ReadFromJsonAsync<ItemQueryModel?>() ?? null!;
            item.Should().NotBeNull();
            item.Id.Should().Be(itemId);
            item.Name.Should().Be(itemDto.Name);
            item.Code.Should().Be(itemDto.Code);
            item.Number.Should().Be(itemDto.Number);
            item.Sku.Should().Be(itemDto.Sku);
            item.QuantityUnit.Should().Be(itemDto.QuantityUnit);
        }

        [Fact]
        public async Task GetItems_Returns_Ok_With_CreatedItems()
        {
            // Arrange
            var itemDto1 = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var createRequest1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequest1.Content = JsonContent.Create(itemDto1);
            var createResponse1 = await _client.SendAsyncWithMasterAuthentication(createRequest1);

            var itemDto2 = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var createRequest2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequest2.Content = JsonContent.Create(itemDto2);
            var createResponse2 = await _client.SendAsyncWithMasterAuthentication(createRequest2);

            // Act
            var getItemsRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Items.GetList);
            var getItemsResponse = await _client.SendAsyncWithMasterAuthentication(getItemsRequest);

            // Assert
            getItemsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var itemList = await getItemsResponse.Content.ReadFromJsonAsync<List<ItemQueryModel>>() ?? null!;
            itemList.Should().NotBeNull();
            itemList.Should().Contain(x =>
                x.Name == itemDto1.Name &&
                x.Code == itemDto1.Code &&
                x.Number == itemDto1.Number &&
                x.Sku == itemDto1.Sku &&
                x.QuantityUnit == itemDto1.QuantityUnit);
            itemList.Should().Contain(x =>
                x.Name == itemDto2.Name &&
                x.Code == itemDto2.Code &&
                x.Number == itemDto2.Number &&
                x.Sku == itemDto2.Sku &&
                x.QuantityUnit == itemDto2.QuantityUnit);

        }

        [Fact]
        public async Task UpdateItem_Returns_Ok()
        {
            // Arrange
            var itemDto1 = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequest.Content = JsonContent.Create(itemDto1);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var itemId = await createResponseMessage.Content.ReadFromJsonAsync<long>();

            // Act
            var itemDto2 = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Items.Update.Replace("{id}", $"{itemId}"));
            updateRequest.Content = JsonContent.Create(itemDto2);
            var updateResponse = await _client.SendAsyncWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", $"{itemId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var item = await getResponse.Content.ReadFromJsonAsync<ItemQueryModel>() ?? null!;
            item.Should().NotBeNull();
            item.Id.Should().Be(itemId);
            item.Name.Should().Be(itemDto2.Name);
            item.Code.Should().Be(itemDto2.Code);
            item.Number.Should().Be(itemDto2.Number);
            item.Sku.Should().Be(itemDto2.Sku);
            item.QuantityUnit.Should().Be(itemDto2.QuantityUnit);
        }

        [Fact]
        public async Task DeleteItem_Returns_Ok()
        {
            // Arrange
            var itemDto = new ItemCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                Code = Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                QuantityUnit = Guid.NewGuid().ToString(),
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequest.Content = JsonContent.Create(itemDto);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var itemId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Items.Delete.Replace("{id}", $"{itemId}"));
            var deleteResponse = await _client.SendAsyncWithMasterAuthentication(deleteRequest);

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", $"{itemId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var item = await getResponse.Content.ReadFromJsonAsync<ItemQueryModel>();
            item.Should().BeNull();
        }

    }
}
