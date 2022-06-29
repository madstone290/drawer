using Drawer.Contract;
using Drawer.Contract.Items;
using Drawer.Contract.Locations;
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

namespace Drawer.IntergrationTest.Items
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

        [Theory]
        [InlineData("CreateItem", "code1", "number1", "sku1", "ea")]
        public async Task CreateItem_Returns_Ok_With_Content(string name, string code,
            string number, string sku, string measurementUnit)
        {
            // Arrange
            var request = new CreateItemRequest(name, code, number, sku, measurementUnit);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateItemResponse>();
            response.Should().NotBeNull();
        }

        [Theory]
        [InlineData("GetItem", "code1", "number1", "sku1", "ea")]
        public async Task GetItem_Returns_Ok_With_CreatedItem(string name, string code,
            string number, string sku, string measurementUnit)
        {
            // Arrange
            var createRequest = new CreateItemRequest(name, code, number, sku, measurementUnit);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateItemResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetItemResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
            getResponse.Code.Should().Be(createRequest.Code);
            getResponse.Number.Should().Be(createRequest.Number);
            getResponse.Sku.Should().Be(createRequest.Sku);
            getResponse.MeasurementUnit.Should().Be(createRequest.MeasurementUnit);
        }

        [Theory]
        [InlineData(
            "UpdateItem1", "c1", "n1", "s1", "ea",
            "UpdateItem2", "c2", "n2", "s2", "ea"
        )]
        public async Task GetItems_Returns_Ok_With_CreatedItems(
            string name1, string code1, string number1, string sku1, string measurementUnit1,
            string name2, string code2, string number2, string sku2, string measurementUnit2)
        {
            // Arrange
            var createRequest1 = new CreateItemRequest(name1, code1, number1, sku1, measurementUnit1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var createRequest2 = new CreateItemRequest(name2, code2, number2, sku2, measurementUnit2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getItemsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Items.GetList);
            var getItemsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getItemsRequestMessage);

            // Assert
            getItemsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getItemsResponse = await getItemsResponseMessage.Content.ReadFromJsonAsync<GetItemsResponse>() ?? null!;
            getItemsResponse.Should().NotBeNull();
            getItemsResponse.Items.Should().NotBeNull();
            getItemsResponse.Items.Should().Contain(x =>
                x.Name == name1 &&
                x.Code == code1 &&
                x.Number == number1 &&
                x.Sku == sku1 &&
                x.MeasurementUnit == measurementUnit1);
            getItemsResponse.Items.Should().Contain(x =>
                x.Name == name2 &&
                x.Code == code2 &&
                x.Number == number2 &&
                x.Sku == sku2 &&
                x.MeasurementUnit == measurementUnit2);

        }

        [Theory]
        [InlineData(
            "UpdateItem1", "c1", "n1", "s1", "ea",
            "UpdateItem2", "c2", "n2", "s2", "ea"
        )]
        public async Task UpdateItem_Returns_Ok(
            string name1, string code1, string number1, string sku1, string measurementUnit1,
            string name2, string code2, string number2, string sku2, string measurementUnit2)
        {
            // Arrange
            var createRequest = new CreateItemRequest(name1, code1, number1, sku1, measurementUnit1);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateItemResponse>() ?? null!;

            // Act
            var updateRequest = new CreateItemRequest(name2, code2, number2, sku2, measurementUnit2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Items.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetItemResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
            getResponse.Code.Should().Be(updateRequest.Code);
            getResponse.Number.Should().Be(updateRequest.Number);
            getResponse.Sku.Should().Be(updateRequest.Sku);
            getResponse.MeasurementUnit.Should().Be(updateRequest.MeasurementUnit);
        }


        [Theory]
        [InlineData("DeleteItem")]
        public async Task DeleteItem_Returns_Ok(string name)
        {
            // Arrange
            var createRequest = new CreateItemRequest(name, null, null, null, null);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Items.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateItemResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.Items.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Items.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
