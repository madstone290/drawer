﻿using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands.ReceiptCommands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class ReceiptApiClient : ApiClient
    {
        public ReceiptApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<List<ReceiptQueryModel>>> GetReceipts(DateTime from, DateTime to)
        {
            var request = new ApiRequest<List<ReceiptQueryModel>>(
                HttpMethod.Get,
                ApiRoutes.Receipts.GetList
                    .AddQuery("From", from.ToString("yyyy-MM-dd"))
                    .AddQuery("To", to.ToString("yyy-MM-dd")));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<ReceiptQueryModel?>> GetReceipt(long id)
        {
            var request = new ApiRequest<ReceiptQueryModel?>(
                HttpMethod.Get,
                ApiRoutes.Receipts.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<long>> AddReceipt(ReceiptAddUpdateCommandModel receipt)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.Receipts.Create,
                receipt);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateReceipt(long id, ReceiptAddUpdateCommandModel receipt)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{id}"),
                receipt);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> DeleteReceipt(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Receipts.Delete.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

       

    }
}
