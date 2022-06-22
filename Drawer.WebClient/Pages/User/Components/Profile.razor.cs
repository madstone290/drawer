using Drawer.Contract;
using Drawer.Contract.Common;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.User.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Drawer.WebClient.Pages.User.Components
{
    public partial class Profile
    {
        public ProfileModel Model { get; set; } = new ProfileModel();
        public ProfileModelValidator Validator { get; set; } = new ProfileModelValidator();
        public MudForm Form { get; set; } = null!;
        public bool FormIsValid { get; set; }

    
        [Inject]
        public ApiClient ApiClient { get; set; } = null!;

        /// <summary>
        /// 오류 메시지
        /// </summary>
        public string? ErrorText { get; set; }


        protected override async Task OnInitializedAsync()
        {

            // 사용자 정보 조회
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            var apiResponseMessage = await ApiClient.SendAsync(new ApiRequestMessage<GetUserResponse>(requstMessage));

            if (apiResponseMessage.IsSuccessful)
            {
                var user = apiResponseMessage.Data;
                Model.Email = user.Email;
                Model.DisplayName = user.DisplayName;
            }
            else
            {
                ErrorText = apiResponseMessage.ErrorMessage;
                if(apiResponseMessage.NeedToLogin)
                {
                    _navigationManager.NavigateTo(Paths.Account.Login);
                }
            }

        }

        async Task SaveClickAsync()
        {
            // 사용자 정보 조회
            var requstMessage = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.User.UpdateUser);
            requstMessage.Content = JsonContent.Create(new UpdateUserRequest(Model.DisplayName!));
            var apiResponseMessage = await ApiClient.SendAsync(new ApiRequestMessage<GetUserResponse>(requstMessage));

            if (apiResponseMessage.IsSuccessful)
            {
                // snackbar 저장 알림
                ErrorText = "저장 성공";
            }
            else
            {
                ErrorText = apiResponseMessage.ErrorMessage;
                if (apiResponseMessage.NeedToLogin)
                {
                    _navigationManager.NavigateTo(Paths.Account.Login);
                }
            }
        }


    }
}
