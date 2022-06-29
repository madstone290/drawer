using Drawer.Contract;
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

namespace Drawer.IntergrationTest.Locations
{
    [Collection(ApiInstanceCollection.Default)]
    public class WorkPlacesControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public WorkPlacesControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData("작업장1", "작업장입니다")]
        [InlineData("작업장2", "작업장입니다")]
        public async Task CreateWorkPlace_Returns_Ok_With_Content(string name, string description) 
        {
            // Arrange
            var request = new CreateWorkPlaceRequest(name, description);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            requestMessage.Content = JsonContent.Create(request);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateWorkPlaceResponse>();
            response.Should().NotBeNull();
            response!.Name.Should().Be(request.Name);
            response!.Description.Should().Be(request.Description);
        }

        [Theory]
        [InlineData("작업장", "작업장입니다")]
        [InlineData("창고", "창고입니다")]
        [InlineData("냉동실", "매장 냉동실입니다")]
        public async Task GetWorkPlace_Returns_Ok_With_CreatedWorkPlace(string name, string description)
        {
            // Arrange
            var createRequest = new CreateWorkPlaceRequest(name, description);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateWorkPlaceResponse>() ?? null!;

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.WorkPlaces.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetWorkPlaceResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(createRequest.Name);
            getResponse.Description.Should().Be(createRequest.Description);
        }

        [Theory]
        [InlineData("작업장1", "작업장1입니다", "창고1", "창고1입니다")]
        public async Task GetWorkPlaces_Returns_Ok_With_CreatedWorkPlaces(
            string name1, string description1,
            string name2, string description2)
        {
            // Arrange
            var createRequest1 = new CreateWorkPlaceRequest(name1, description1);
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            createRequestMessage1.Content = JsonContent.Create(createRequest1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var createRequest2 = new CreateWorkPlaceRequest(name2, description2);
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            createRequestMessage2.Content = JsonContent.Create(createRequest2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getWorkPlacesRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.WorkPlaces.GetList);
            var getWorkPlacesResponseMessage = await _client.SendAsyncWithMasterAuthentication(getWorkPlacesRequestMessage);

            // Assert
            getWorkPlacesResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getWorkPlacesResponse = await getWorkPlacesResponseMessage.Content.ReadFromJsonAsync<GetWorkPlacesResponse>() ?? null!;
            getWorkPlacesResponse.Should().NotBeNull();
            getWorkPlacesResponse.WorkPlaces.Should().NotBeNull();
            getWorkPlacesResponse.WorkPlaces.Should().Contain(x=> x.Name == name1 && x.Description == description1);
            getWorkPlacesResponse.WorkPlaces.Should().Contain(x => x.Name == name2 && x.Description == description2);
        }

        [Theory]
        [InlineData("작업장", "작업장입니다", "작업장 수정", "수정된 작업장입니다" )]
        public async Task UpdateWorkPlace_Returns_Ok(string name, string description, string name2, string description2)
        {
            // Arrange
            var createRequest = new CreateWorkPlaceRequest(name, description);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateWorkPlaceResponse>() ?? null!;

            // Act
            var updateRequest = new CreateWorkPlaceRequest(name2, description2);
            var updateRequestMessage = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.WorkPlaces.Update.Replace("{id}", createResponse.Id.ToString()));
            updateRequestMessage.Content = JsonContent.Create(updateRequest);
            var updateResponseMessage = await _client.SendAsyncWithMasterAuthentication(updateRequestMessage);

            // Assert
            updateResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, 
                ApiRoutes.WorkPlaces.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var getResponse = await getResponseMessage.Content.ReadFromJsonAsync<GetWorkPlaceResponse>() ?? null!;
            getResponse.Should().NotBeNull();
            getResponse.Id.Should().Be(createResponse.Id);
            getResponse.Name.Should().Be(updateRequest.Name);
            getResponse.Description.Should().Be(updateRequest.Description);
        }

        [Theory]
        [InlineData("작업장", "작업장입니다")]
        public async Task DeleteWorkPlace_Returns_Ok(string name, string description)
        {
            // Arrange
            var createRequest = new CreateWorkPlaceRequest(name, description);
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            createRequestMessage.Content = JsonContent.Create(createRequest);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var createResponse = await createResponseMessage.Content.ReadFromJsonAsync<CreateWorkPlaceResponse>() ?? null!;

            // Act
            var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete,
                ApiRoutes.WorkPlaces.Delete.Replace("{id}", createResponse.Id.ToString()));
            var deleteResponseMessage = await _client.SendAsyncWithMasterAuthentication(deleteRequestMessage);

            // Assert
            deleteResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.WorkPlaces.Get.Replace("{id}", createResponse.Id.ToString()));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
