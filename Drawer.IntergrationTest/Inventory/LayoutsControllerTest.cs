﻿using Drawer.Application.Services.Inventory.CommandModels;
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

        async Task<long> CreateRootGroup()
        {
            var requestContent = new LocationGroupAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.LocationGroups.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var groupId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return groupId;
        }

        async Task<long> CreateLocation()
        {
            var requestContent = new LocationAddCommandModel()
            {
                Name = Guid.NewGuid().ToString(),
                GroupId = await CreateRootGroup(),
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Locations.Add);
            requestMessage.Content = JsonContent.Create(requestContent);
            var ResponseMessage = await _client.SendWithMasterAuthentication(requestMessage);
            var locationId = await ResponseMessage.Content.ReadFromJsonAsync<long>();
            return locationId;
        }


        [Fact]
        public async Task CreateLayout_Returns_Ok()
        {
            // Arrange
            var locationId = await CreateRootGroup();
            var requestContent = new LayoutEditCommandModel()
            {
                LocationGroupId = locationId
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            requestMessage.Content = JsonContent.Create(requestContent);

            // Act
            var responseMessage = await _client.SendWithMasterAuthentication(requestMessage);

            // Assert
            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLayout_Returns_Ok_With_CreatedLayout()
        {
            // Arrange
            var groupId = await CreateRootGroup();
            var requestContent = new LayoutEditCommandModel()
            {
                LocationGroupId = groupId
            };
            var createRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            createRequestMessage.Content = JsonContent.Create(requestContent);
            var createResponseMessage = await _client.SendWithMasterAuthentication(createRequestMessage);

            // Act
            var getRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Layouts.GetByLocationGroup.Replace("{groupId}", $"{groupId}"));
            var getResponseMessage = await _client.SendWithMasterAuthentication(getRequestMessage);

            // Assert
            getResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layout = await getResponseMessage.Content.ReadFromJsonAsync<LayoutQueryModel>() ?? null!;
            layout.Should().NotBeNull();
            layout.LocationId.Should().Be(requestContent.LocationGroupId);
            // TODO 아이템 비교할 것
        }

        [Fact]
        public async Task GetLayouts_Returns_Ok_With_CreatedLayouts()
        {
            // Arrange
            var locationId1 = await CreateRootGroup();
            var requestContent1 = new LayoutEditCommandModel()
            {
                LocationGroupId = locationId1,
            };
            var createRequestMessage1 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            createRequestMessage1.Content = JsonContent.Create(requestContent1);
            var createResponseMessage1 = await _client.SendWithMasterAuthentication(createRequestMessage1);

            var locationId2 = await CreateRootGroup();
            var requestContent2 = new LayoutEditCommandModel()
            {
                LocationGroupId = locationId2,
            };
            var createRequestMessage2 = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            createRequestMessage2.Content = JsonContent.Create(requestContent2);
            var createResponseMessage2 = await _client.SendWithMasterAuthentication(createRequestMessage2);

            // Act
            var getLayoutsRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Layouts.GetList);
            var getLayoutsResponseMessage = await _client.SendWithMasterAuthentication(getLayoutsRequestMessage);

            // Assert
            getLayoutsResponseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layoutList = await getLayoutsResponseMessage.Content.ReadFromJsonAsync<List<LayoutQueryModel>>() ?? null!;
            layoutList.Should().NotBeNull();
            layoutList.Should().Contain(x =>
                x.LocationId == requestContent1.LocationGroupId);
            layoutList.Should().Contain(x =>
                x.LocationId == requestContent2.LocationGroupId);
        }

        [Fact]
        public async Task UpdateLayout_Returns_Ok()
        {
            // Arrange
            var groupId = await CreateRootGroup();
            var createContent = new LayoutEditCommandModel()
            {
                LocationGroupId = groupId,
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);

            // Act
            var updateContent = new LayoutEditCommandModel()
            {
                LocationGroupId = groupId,
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
            var updateRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,
                ApiRoutes.Layouts.GetByLocationGroup.Replace("{groupId}", $"{groupId}"));
            var getResponse = await _client.SendWithMasterAuthentication(getRequest);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var layout = await getResponse.Content.ReadFromJsonAsync<LayoutQueryModel?>() ?? null!;
            layout.Should().NotBeNull();
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
            var locationId = await CreateRootGroup();
            var createContent = new LayoutEditCommandModel()
            {
                LocationGroupId = locationId,
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Layouts.Edit);
            createRequest.Content = JsonContent.Create(createContent);
            var createResponse = await _client.SendWithMasterAuthentication(createRequest);

            // Act
            var updateContent = new LayoutEditCommandModel()
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
            var updateRequest = new HttpRequestMessage(HttpMethod.Post,
                ApiRoutes.Layouts.Edit.Replace("{locationId}", $"{locationId}"));
            updateRequest.Content = JsonContent.Create(updateContent);
            var updateResponse = await _client.SendWithMasterAuthentication(updateRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
