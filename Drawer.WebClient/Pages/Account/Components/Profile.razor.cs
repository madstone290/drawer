using Drawer.Contract;
using Drawer.Contract.Common;
using Drawer.Contract.UserInformation;
using Drawer.WebClient.Pages.Account.Models;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account.Components
{
    public partial class Profile
    {
        public ProfileModel Model { get; set; } = new ProfileModel();
        public ProfileModelValidator Validator { get; set; } = new ProfileModelValidator();
        public MudForm Form { get; set; } = null!;
        public bool FormIsValid { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; } = null!;
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        /// <summary>
        /// 오류 메시지
        /// </summary>
        public string? ErrorText { get; set; }


        protected override async Task OnInitializedAsync()
        {

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var at = state.User.Claims.FirstOrDefault(x => x.Type == "AccessToken").Value;

            // 사용자 정보 조회
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.GetUser);
            requstMessage.Headers.Add("Authorization", $"bearer {at}");

            var responseMessage = await HttpClient.SendAsync(requstMessage);
            if (responseMessage.IsSuccessStatusCode)
            {
                var user = await responseMessage.Content.ReadFromJsonAsync<GetUserResponse>();
                Model.Email = user!.Email;
                Model.DisplayName = user!.DisplayName;
            }
            else if(responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest
                || responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorText = error?.Message;
            }
            else
            {
                ErrorText = responseMessage.StatusCode.ToString();
            }
        }

        async Task SaveClickAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var at = state.User.Claims.FirstOrDefault(x => x.Type == "AccessToken").Value;

            // 사용자 정보 조회
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.User.UpdateUser);
            requstMessage.Headers.Add("Authorization", $"bearer {at}");
            requstMessage.Content = JsonContent.Create(new UpdateUserRequest(Model.DisplayName!));

            var responseMessage = await HttpClient.SendAsync(requstMessage);
            if (responseMessage.IsSuccessStatusCode)
            {
                // snackbar 저장 알림
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest || responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorText = error?.Message;
            }
            else
            {
                ErrorText = responseMessage.StatusCode.ToString();
            }
        }


    }
}
