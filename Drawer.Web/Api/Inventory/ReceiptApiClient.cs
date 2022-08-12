using Drawer.Application.Services.Inventory.CommandModels;
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

        public async Task<ApiResponse<long>> AddReceipt(ReceiptCommandModel receipt)
        {
            var request = new ApiRequest<long>(
                HttpMethod.Post,
                ApiRoutes.Receipts.Add,
                receipt);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<List<long>>> BatchAddReceipt(List<ReceiptCommandModel> receiptList)
        {
            var request = new ApiRequest<List<long>>(
                HttpMethod.Post,
                ApiRoutes.Receipts.BatchAdd,
                receiptList);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateReceipt(long id, ReceiptCommandModel receipt)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{id}"),
                receipt);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> RemoveReceipt(long id)
        {
            var request = new ApiRequest(
                HttpMethod.Delete,
                ApiRoutes.Receipts.Remove.Replace("{id}", $"{id}"));
                
            return await SendAsync(request);
        }

       

    }
}
