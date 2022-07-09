using Drawer.Api.Controllers;
using Drawer.Application.Services.Authentication.Commands;
using Drawer.Contract;
using Drawer.Contract.Authentication;
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
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand(request.Email, request.Password, request.DisplayName);
            var result = await _mediator.Send(command);
            return Ok(new RegisterResponse(result.UserId, result.Email, result.DisplayName));
        }

        /// <summary>
        /// 가입 확인 이메일을 전송한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
        {
            // 개발환경에서는 호스팅 Uri를 사용한다.
            // 운영환경에서는 Api서버의 도메인을 이용한다.
            var returnUri = string.Empty;
            if (_environment.IsDevelopment())
            {
                returnUri = Url.RouteUrl(nameof(VerifyEmailAsync), new { request.RedirectUri }, HttpContext.Request.Scheme)!;
            }
            else
            {
                returnUri = _configuration["DrawerApiDomain"] +
                    ApiRoutes.Account.ConfirmEmail + $"?RedirectUri={Uri.EscapeDataString(request.RedirectUri)}";
            }

            var command = new ConfirmEmailCommand(request.Email, returnUri);
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
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);
            return Ok(new LoginResponse(result.IsCompanyMemeber, result.IsCompanyOwner, result.AccessToken, result.RefreshToken));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Account.Refresh)]
        [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest model)
        {
            var command = new RefreshCommand(model.Email, model.RefreshToken);
            var result = await _mediator.Send(command);
            return Ok(new RefreshResponse(result.AccessToken));
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
