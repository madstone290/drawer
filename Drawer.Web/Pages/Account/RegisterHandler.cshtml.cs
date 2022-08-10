using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Shared;
using Drawer.Shared.Contracts.Common;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.Web.Pages.Account
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
			var registerResponse = await _httpClient.PostAsJsonAsync(
                ApiRoutes.Account.Register, 
				new RegisterCommandModel(email, password, displayName));

			if (!registerResponse.IsSuccessStatusCode)
			{
				var error = await registerResponse.Content.ReadFromJsonAsync<ErrorResponse>();
				return Redirect(Paths.Account.Register.AddQuery("error", error!.Message));
			}

            return Redirect(Paths.Account.ConfirmEmail.AddQuery("email", email));

		}
    }
}
