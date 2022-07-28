using Blazored.LocalStorage;
using Drawer.Web.Authentication;
using Drawer.Web.Frontend;
using Drawer.Web.Pages.Account.Models;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Drawer.Web.Pages.Account
{
    public partial class Login  
    {
        /// <summary>
        /// 로그인핸들러에서 발생한 에러.
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        /// <summary>
        /// 로그인이 완료된 후 리디렉트할 곳의 Uri
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? RedirectUri { get; set; }

        public LoginModel Model { get; set; } = new LoginModel();
        public LoginModelValidator Validator { get; set; } = new LoginModelValidator();
        public MudForm Form { get; private set; } = null!;

        [Inject] IAuthenticationManager AuthenticationStateProvider { get; set; } = null!;
        [Inject] ILocalStorage LocalStorage { get; set; } = null!;
        

        protected override async Task OnInitializedAsync()
        {
            // 로그인 상태인 경우 홈으로 리디렉트
            var state = await AuthenticationStateProvider.GetUserStateAsync();
            if (state.IsAuthenticated)
            { 
                NavManager.NavigateTo(Paths.Base);
            }

            // 로그인 옵션 불러오기
            await LoadOptionsAsync();
        }

        async Task HandlerKeyboard(KeyboardEventArgs e)
        {
            if(e.Key == "Enter")
            {
                await SubmitAsync();
            }
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
                    .AddQueryParam("password", Model.Password!)
                    .AddQueryParam("redirectUri", RedirectUri);

                NavManager.NavigateTo(navigationUri, true);
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
                await LocalStorage.SetItemAsync(Constants.LocalStorageKeys.Email, Model.Email!);
            }
            else
            {
                await LocalStorage.RemoveItemAsync(Constants.LocalStorageKeys.Email);
            }
        }

        /// <summary>
        /// 로그인 옵션을 불러온다.
        /// </summary>
        /// <returns></returns>
        async Task LoadOptionsAsync()
        {
            if(await LocalStorage.ContainKeyAsync(Constants.LocalStorageKeys.Email))
            {
                var email = await LocalStorage.GetItemAsync(Constants.LocalStorageKeys.Email);

                Model.Email = email;
                Model.RememberEmail = true;
            }
        }

    }
}


