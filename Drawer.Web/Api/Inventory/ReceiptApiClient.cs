using Drawer.Contract;
using Drawer.Contract.Inventory;
using Drawer.Web.Authentication;
using Drawer.Web.Utils;

namespace Drawer.Web.Api.Inventory
{
    public class ReceiptApiClient : ApiClient
    {
        public ReceiptApiClient(HttpClient httpClient, ITokenManager tokenManager) : base(httpClient, tokenManager)
        {

        }

        public async Task<ApiResponse<GetReceiptsResponse>> GetReceipts(DateTime from, DateTime to)
        {
            var request = new ApiRequest<GetReceiptsResponse>(
                HttpMethod.Get,
                ApiRoutes.Receipts.GetList
                    .AddQuery("From", from.ToString("yyyy-MM-dd"))
                    .AddQuery("To", to.ToString("yyy-MM-dd")));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<GetReceiptResponse>> GetReceipt(long id)
        {
            var request = new ApiRequest<GetReceiptResponse>(
                HttpMethod.Get,
                ApiRoutes.Receipts.Get.Replace("{id}", $"{id}"));

            return await SendAsync(request);
        }

        public async Task<ApiResponse<CreateReceiptResponse>> AddReceipt(CreateReceiptRequest content)
        {
            var request = new ApiRequest<CreateReceiptResponse>(
                HttpMethod.Post,
                ApiRoutes.Receipts.Create,
                content);

            return await SendAsync(request);
        }

        public async Task<ApiResponse<Unit>> UpdateReceipt(long id, UpdateReceiptRequest content)
        {
            var request = new ApiRequest(
                HttpMethod.Put,
                ApiRoutes.Receipts.Update.Replace("{id}", $"{id}"),
                content);

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
