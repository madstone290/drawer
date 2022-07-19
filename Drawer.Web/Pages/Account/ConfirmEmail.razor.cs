using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Account
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

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        async Task Send()
        {
            var registerCompletedUri =  NavManager.BaseUri.AddPath(Paths.Account.RegisterCompleted);
            var confirmResponseMessage = await HttpClient.PostAsJsonAsync(ApiRoutes.Account.ConfirmEmail,
                new ConfirmEmailRequest(Email, registerCompletedUri!));

            if (!confirmResponseMessage.IsSuccessStatusCode)
            {
                var result = await confirmResponseMessage.Content.ReadNullableJsonAsync<ErrorResponse>();
                Error = result.IsSuccessful 
                    ? result.Data.Message
                    : confirmResponseMessage.StatusCode.ToString();
            }
            else
            {
                IsEmailSent = true;
            }
            
        }
    }
}
