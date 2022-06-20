using Drawer.Contract.Authentication;
using Drawer.Contract.Common;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.WebClient.Pages.Account
{
    public class RegisterHandlerModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterHandlerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.HttpClient.DrawerApi);
        }

        /// <summary>
        /// ȸ�������� �����Ѵ�.
        /// </summary>
        /// <param name="returnUri">���� �� ������ �߻����� �� ���ư� URI</param>
        /// <param name="redirectUri">������ �Ϸ�� �� ���ư� URI</param>
        /// <param name="displayName">ȸ�� �̸�</param>
        /// <param name="email">�α��� �̸���</param>
        /// <param name="password">�α��� ��й�ȣ</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string returnUri, string redirectUri, string displayName, string email, string password)
        {
			var registerResponseMessage = await _httpClient.PostAsJsonAsync("/api/account/register", 
				new RegisterRequest(email, password, displayName));

			if (!registerResponseMessage.IsSuccessStatusCode)
			{
				var error = await registerResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
				return Redirect(returnUri + $"?error={Uri.EscapeDataString(error!.Message)}");
			}

            var registerCompletedUri = UriUtils.GetAbsoluteUri(HttpContext.Request, "/acount/registerCompleted)");
            var confirmResponseMessage = await _httpClient.PostAsJsonAsync("/api/account/confirmemail", 
                new ConfirmEmailRequest(email, registerCompletedUri!));

            if (!confirmResponseMessage.IsSuccessStatusCode)
            {
                var error = await confirmResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                return Redirect(returnUri + $"?error={Uri.EscapeDataString(error!.Message)}");
            }

            return Redirect(redirectUri);

		}
    }
}
