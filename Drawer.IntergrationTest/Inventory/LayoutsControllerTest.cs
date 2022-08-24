using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
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
    public class LayoutsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public LayoutsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        async Task<long> CreateRootLocation()
        {
            var requestContent = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                IsGroup = true
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }

        async Task<long> CreateLocation()
        {
            var requestContent = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }


        [Fact]
        public async Task CreateLayout_Returns_Ok()
        {
            // Arrange
            var locationId = await CreateRootLocation();
            var requestContent = new LayoutAddCommandModel()
            {
                LocationId = locationId
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendAsyncWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layoutId = await responseMessage.Content.ReadFromJsonAsync<long>();
            layoutId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetLayout_Returns_Ok_With_CreatedLayout()
        {
            // Arrange
            var locationId = await CreateRootLocation();
            var requestContent = new LayoutAddCommandModel()
            {
                LocationId = locationId
            };
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            createRequestMessage.Content = JsonContent.Create(requestContent);
            var createResponseMessage = await _client.SendAsyncWithMasterAuthentication(createRequestMessage);
            var layoutId = await createResponseMessage.Content.ReadFromJsonAsync<long>();

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Layouts.Get.Replace("{id}", $"{layoutId}"));
            var getResponseMessage = await _client.SendAsyncWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layout = await getResponseMessage.Content.ReadFromJsonAsync<LayoutQueryModel>() ?? null!;
            layout.Should().NotBeNull();
            layout.Id.Should().Be(layoutId);
            layout.LocationId.Should().Be(requestContent.LocationId);
            // TODO 아이템 비교할 것
        }

        [Fact]
        public async Task GetLayouts_Returns_Ok_With_CreatedLayouts()
        {
            // Arrange
            var locationId1 = await CreateRootLocation();
            var requestContent1 = new LayoutAddCommandModel()
            {
                LocationId = locationId1,
            };
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            createRequestMessage1.Content = JsonContent.Create(requestContent1);
            var createResponseMessage1 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage1);

            var locationId2 = await CreateRootLocation();
            var requestContent2 = new LayoutAddCommandModel()
            {
                LocationId = locationId2,
            };
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            createRequestMessage2.Content = JsonContent.Create(requestContent2);
            var createResponseMessage2 = await _client.SendAsyncWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLayoutsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Layouts.GetList);
            var getLayoutsResponseMessage = await _client.SendAsyncWithMasterAuthentication(getLayoutsRequestMessage);

            // Assert
            getLayoutsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layoutList = await getLayoutsResponseMessage.Content.ReadFromJsonAsync<List<LayoutQueryModel>>() ?? null!;
            layoutList.Should().NotBeNull();
            layoutList.Should().Contain(x =>
                x.LocationId == requestContent1.LocationId);
            layoutList.Should().Contain(x =>
                x.LocationId == requestContent2.LocationId);
        }

        [Fact]
        public async Task UpdateLayout_Returns_Ok()
        {
            // Arrange
            var locationId = await CreateRootLocation();
            var createContent = new LayoutAddCommandModel()
            {
                LocationId = locationId,
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var layoutId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var updateContent = new LayoutUpdateCommandModel()
            {
                ItemList = new List<LayoutItem>()
                {
                    new LayoutItem()
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ConnectedLocations = new long[]
                        {
                            await CreateLocation(),
                            await CreateLocation()
                        },
                        Degree = LayoutItemOptions.Degree.Column,
                        HAlignment = LayoutItemOptions.HAlignment.Center,
                        VAlignment = LayoutItemOptions.VAlignment.Center,
                        Shape = LayoutItemOptions.Shape.Rect,
                        Text = "공장",
                        IsPattern = false,
                        PatternImageId = null,
                        Height = 100,
                        Width = 100,
                        Left = 30, 
                        Top = 50, 
                        BackColor = "#eeeeee",
                        FontSize = 15,
                    },
                    new LayoutItem()
                    {
                        ConnectedLocations = new long[]
                        {
                            await CreateLocation(),
                            await CreateLocation()
                        },
                        ItemId = Guid.NewGuid().ToString(),
                        Degree = LayoutItemOptions.Degree.Column,
                        HAlignment = LayoutItemOptions.HAlignment.Center,
                        VAlignment = LayoutItemOptions.VAlignment.Center,
                        Shape = LayoutItemOptions.Shape.Rect,
                        Text = "공장",
                        IsPattern = false,
                        PatternImageId = null,
                        Height = 200,
                        Width = 50,
                        Left = 50,
                        Top = 200,
                        BackColor = "#eeeeee",
                        FontSize = 15,
                    }
                }
            };
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Layouts.Update.Replace("{id}", $"{layoutId}"));
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendAsyncWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Layouts.Get.Replace("{id}", $"{layoutId}"));
            var getResponse = await _client.SendAsyncWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layout = await getResponse.Content.ReadFromJsonAsync<LayoutQueryModel?>() ?? null!;
            layout.Should().NotBeNull();
            layout.Id.Should().Be(layoutId);
            layout.ItemList.Should().Contain(updateContent.ItemList);
        }

        [Theory]
        [InlineData("rect1", "row", "center", "center")]
        [InlineData("rect", "row1", "center", "center")]
        [InlineData("rect", "row", "center1", "center")]
        [InlineData("rect", "row", "center", "center1")]
        public async Task UpdateLayout_With_WrongOptions_Returns_BadRequest(string shape, string degree, string hAlignment, string vAlignment)
        {
            // Arrange
            var locationId = await CreateRootLocation();
            var createContent = new LayoutAddCommandModel()
            {
                LocationId = locationId,
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Add);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendAsyncWithMasterAuthentication(createRequest);
            var layoutId = await createResponse.Content.ReadFromJsonAsync<long>();

            // Act
            var updateContent = new LayoutUpdateCommandModel()
            {
                ItemList = new List<LayoutItem>()
                {
                    new LayoutItem()
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ConnectedLocations = new long[]
                        {
                            await CreateLocation(),
                            await CreateLocation()
                        },
                        Shape = shape, 
                        Degree = degree,
                        HAlignment = hAlignment,
                        VAlignment = vAlignment,
                        Text = "공장",
                        IsPattern = false,
                        PatternImageId = null,
                        Height = 100,
                        Width = 100,
                        Left = 30,
                        Top = 50,
                        BackColor = "#eeeeee",
                        FontSize = 15,
                    }
                }
            };
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,
                ApiRoutes.Layouts.Update.Replace("{id}", $"{layoutId}"));
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendAsyncWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
