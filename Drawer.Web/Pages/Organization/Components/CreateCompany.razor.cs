using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Organization.Components
{
    public partial class CreateCompany
    {
        private MudForm? _form;
        private bool _isFormValid;

        public CreateCompanyModel _company = new();
        public CreateCompanyModel.Validator _validator = new();

        [Inject]
        public CompanyApiClient CompanyApiClient { get; set; } = null!;

        async Task Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();

            if (!_isFormValid)
                return;

            var companyDto = new CompanyCommandModel()
            {
                Name = _company.Name
            };

            var response = await CompanyApiClient.CreateCompany(companyDto);

            // RazorPage로 리디렉트하기 때문에 성공출력은 하지 않는다.
            if (Snackbar.CheckFail(response))
            {
                // 쿠키 및 JWT 인증상태 갱신
                var navigationUri = Paths.Account.Refresh
                    .AddQuery("redirectUri", Paths.CompanyHome)
                    .AddQuery("isCompanyMember", true.ToString())
                    .AddQuery("isCompanyOwner", true.ToString());

                NavManager.NavigateTo(navigationUri, true);
            }
        }

    }
}


