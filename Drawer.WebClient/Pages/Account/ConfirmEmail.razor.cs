using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.WebClient.Pages.Account
{
    public partial class ConfirmEmail
    {
        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// 이메일 전송 여부
        /// </summary>
        public bool IsEmailSent { get; set; }

        /// <summary>
        /// 대상 이메일
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string Email { get; set; } = null!;

        [Inject] HttpClient HttpClient { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        async Task Send()
        {
            var registerCompletedUri =  NavigationManager.BaseUri.AddPath(Paths.Account.RegisterCompleted);
            var confirmResponseMessage = await HttpClient.PostAsJsonAsync(Paths.Account.ConfirmEmail,
                new ConfirmEmailRequest(Email, registerCompletedUri!));

            if (!confirmResponseMessage.IsSuccessStatusCode)
            {
                var error = await confirmResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                Error = error!.Message;
            }

            IsEmailSent = true;
        }
    }
}
