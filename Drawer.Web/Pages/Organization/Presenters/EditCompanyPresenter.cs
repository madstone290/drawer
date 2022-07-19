using Drawer.Contract;
using Drawer.Contract.Organization;
using Drawer.Web.Api;
using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.Organization.Presenters
{
    public class EditCompanyPresenter : SnackbarPresenter
    {
        private readonly CompanyApiClient _apiClient;

        public IEditCompanyView View { get; set; } = null!;

        public EditCompanyPresenter(CompanyApiClient apiClient, ISnackbar snackbar) : base(snackbar)
        {
            _apiClient = apiClient;
        }

        public async Task<ApiResponse<CreateCompanyResponse>> CreateCompanyAsync()
        {
            var response = await _apiClient.CreateCompany(View.Model.Name, View.Model.PhoneNumber);
            // RazorPage로 리디렉트하기 때문에 성공출력은 하지 않는다.
            CheckFail(response);

            if(response.IsSuccessful)
            {
                View.CloseView();
            }
            return response;
        }

        public async Task<ApiResponse<Unit>> UpdateCompanyAsync()
        {
            var response = await _apiClient.UpdateCompany(View.Model.Name, View.Model.PhoneNumber);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
            {
                View.CloseView();
            }
            return response;
        }
    }
}
