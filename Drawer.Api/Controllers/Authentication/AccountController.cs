using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Application.Services.Authentication.Commands;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Authentication
{
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public AccountController(IMediator mediator, ILogger<AccountController> logger, 
            IWebHostEnvironment environment, IConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.Register)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterAsync([FromBody] Application.Services.Authentication.CommandModels.RegisterCommandModel register)
        {
            var command = new RegisterCommand(register);
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 가입 확인 이메일을 전송한다.
        /// </summary>
        /// <param name="confirmation"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] EmailConfirmationCommandModel confirmation)
        {
            // 개발환경에서는 호스팅 Uri를 사용한다.
            // 운영환경에서는 Api서버의 도메인을 이용한다.
            var returnUri = string.Empty;
            if (_environment.IsDevelopment())
            {
                returnUri = Url.RouteUrl(nameof(VerifyEmailAsync), new { confirmation.RedirectUri }, HttpContext.Request.Scheme)!;
            }
            else
            {
                returnUri = _configuration["DrawerApiDomain"] +
                    ApiRoutes.Account.ConfirmEmail + $"?RedirectUri={Uri.EscapeDataString(confirmation.RedirectUri)}";
            }

            confirmation.RedirectUri = returnUri;
            var command = new ConfirmEmailCommand(confirmation);
            var result = await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 가입확인 이메일 링크를 클릭한다.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.ConfirmEmail, Name = nameof(VerifyEmailAsync))]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string email, [FromQuery] string token, [FromQuery] string redirectUri)
        {
            var command = new VerifyEmailCommand(email, token);
            var result = await _mediator.Send(command);
            return Redirect(redirectUri);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.Login)]
        [ProducesResponseType(typeof(LoginResponseCommandModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommandModel login)
        {
            var command = new LoginCommand(login);
            var loginResponse = await _mediator.Send(command);
            return Ok(loginResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.Refresh)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshCommandModel refresh)
        {
            var command = new RefreshCommand(refresh);
            var accessToken = await _mediator.Send(command);
            return Ok(accessToken);
        }

        /// <summary>
        /// API 권한 테스트 용도로만 사용한다
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Account.SecurityTest)]
        public IActionResult SecurityTest()
        {
            return Ok("You are authorized");
        }

    }
}
