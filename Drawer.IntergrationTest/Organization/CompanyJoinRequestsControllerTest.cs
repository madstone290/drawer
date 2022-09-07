using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Commands;
using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Drawer.IntergrationTest.Organization
{
    [Collection(ApiInstanceCollection.Default)]
    public class CompanyJoinRequestsControllerTest
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _outputHelper;

        public CompanyJoinRequestsControllerTest(ApiInstance apiInstance, ITestOutputHelper outputHelper)
        {
            _client = apiInstance.Client;
            _outputHelper = outputHelper;
        }

        //private async Task<UserQueryModel> CreateOwner()
        //{
        //    return default!;
        //}

        //private async Task<UserQueryModel> CreateUser()
        //{
        //    return default!;
        //}

        //public async void AddRequest_Return_Ok()
        //{
        //    //var ownerEmail = await CreateOwner();


        //    //var requestDto = new CompanyJoinRequestCommandModel()
        //    //{
        //    //    OwnerEmail = ownerEmail
        //    //};
        //    //var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.CompanyJoinRequests.Add);
        //    //request.Content = JsonContent.Create(requestDto);
        //    //_client.SendAsyncWithMasterAuthentication()

        //}



    }
}
