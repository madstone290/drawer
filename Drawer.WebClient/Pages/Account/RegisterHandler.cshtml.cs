using Drawer.Contract;
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

        public RegisterHandlerModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
        public async Task<IActionResult> OnGetAsync(string displayName, string email, string password)
        {
			var registerResponseMessage = await _httpClient.PostAsJsonAsync(ApiRoutes.Account.Register, 
				new RegisterRequest(email, password, displayName));

			if (!registerResponseMessage.IsSuccessStatusCode)
			{
				var error = await registerResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
				return Redirect(Paths.Account.Register.AddQueryParam("error", error!.Message));
			}

            return Redirect(Paths.Account.ConfirmEmail.AddQueryParam("email", email));

		}
    }
}
