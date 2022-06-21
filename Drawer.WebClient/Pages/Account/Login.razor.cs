using Drawer.WebClient.Pages.Account.Models;
using Drawer.WebClient.Shared;
using Drawer.WebClient.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account
{
    public partial class Login
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        public ContentError ContentError { get; set; } = null!;


        public LoginModel Model { get; set; } = new LoginModel();
        public LoginModelValidator Validator { get; set; } = new LoginModelValidator();
        public MudForm Form { get; private set; } = null!;

        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Inject] ProtectedLocalStorage LocalStorage { get; set; } = null!;
        

        protected override async Task OnInitializedAsync()
        {
            // 로그인 상태인 경우 홈으로 리디렉트
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated == true)
            {
                NavigationManager.NavigateTo(Paths.Base);
            }

            // 로그인 옵션 불러오기
            await LoadOptionsAsync();

            ContentError.UpdateText(Error);


        }


        async Task SubmitAsync()
        {
            await Form.Validate();
            if (Form.IsValid)
            {
                // 설정 저장하기
                await SaveOptionsAsync();

                // 로그인 진행
                var navigationUri = Paths.Account.LoginHandler
                    .AddQueryParam("email", Model.Email!)
                    .AddQueryParam("password", Model.Password!);

                NavigationManager.NavigateTo(navigationUri, true);
            }
        }

        /// <summary>
        /// 로그인 옵션을 저장한다.
        /// </summary>
        /// <returns></returns>
        async Task SaveOptionsAsync()
        {
            if (Model.RememberEmail)
            {
                await LocalStorage.SetAsync(Constants.LocalStorageKeys.Email, Model.Email!);
            }
            else
            {
                await LocalStorage.DeleteAsync(Constants.LocalStorageKeys.Email);
            }
        }

        /// <summary>
        /// 로그인 옵션을 불러온다.
        /// </summary>
        /// <returns></returns>
        async Task LoadOptionsAsync()
        {
            var emailStorageResult = await LocalStorage.GetAsync<string>(Constants.LocalStorageKeys.Email);
            if (emailStorageResult.Success)
            {
                Model.Email = emailStorageResult.Value;
                Model.RememberEmail = true;
            }
        }

    }
}


