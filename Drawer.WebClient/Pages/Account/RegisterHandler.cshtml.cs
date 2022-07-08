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
