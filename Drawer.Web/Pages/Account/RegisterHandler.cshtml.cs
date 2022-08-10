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
        /// 회원가입을 진행한다.
        /// </summary>
        /// <param name="returnUri">가입 중 오류가 발생했을 때 돌아갈 URI</param>
        /// <param name="redirectUri">가입이 완료된 후 돌아갈 URI</param>
        /// <param name="displayName">회원 이름</param>
        /// <param name="email">로그인 이메일</param>
        /// <param name="password">로그인 비밀번호</param>
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
