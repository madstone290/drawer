using Drawer.Web.Authentication;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Organization.Components
{
    public partial class RegisterCompany
    {
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IAuthenticationManager AuthenticationManager { get; set; } = null!;
        [Inject] public ITokenManager TokenManager { get; set; } = null!;
        async Task CreateCompanyBtnClickAsync()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditCompanyDialog.EditMode), EditMode.Add }
            };
            var dialog = DialogService.Show<EditCompanyDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                // 쿠키 및 JWT 인증상태 갱신
                var navigationUri = Paths.Account.Refresh
                    .AddQuery("redirectUri", NavManager.Uri)
                    .AddQuery("isCompanyMember", true.ToString())
                    .AddQuery("isCompanyOwner", true.ToString());

                NavManager.NavigateTo(navigationUri, true);
            }
        }

        void RequestBtnClick()
        {

        }
    }
}
