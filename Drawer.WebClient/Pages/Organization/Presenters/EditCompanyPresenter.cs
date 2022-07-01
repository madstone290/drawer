using Drawer.Contract;
using Drawer.Contract.Organization;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.Organization.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.Organization.Presenters
{
    public class EditCompanyPresenter : SnackbarPresenter
    {
        public IEditCompanyView View { get; set; } = null!;

        public EditCompanyPresenter(ApiClient apiClient, ISnackbar snackbar) : base(apiClient, snackbar)
        {
        }
        
        public async Task<ApiResponseMessage<CreateCompanyResponse>> CreateCompanyAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Company.Create);
            requstMessage.Content = JsonContent.Create(new CreateCompanyRequest(View.Model.Name, View.Model.PhoneNumber));
            
            // RazorPage로 리디렉트하기 때문에 메시지가 제대로 표시되지 않는다.
            ShowSuccessMessage = false;

            return await SaveAsync(new ApiRequestMessage<CreateCompanyResponse>(requstMessage));
        }

        public async Task<ApiResponseMessage<Unit>> UpdateCompanyAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Company.Update);
            requstMessage.Content = JsonContent.Create(new UpdateCompanyRequest(View.Model.Name, View.Model.PhoneNumber));

            ShowSuccessMessage = true;

            return await SaveAsync(new ApiRequestMessage<Unit>(requstMessage));
        }
    }
}
