using Drawer.Api.Controllers;
using Drawer.Application.Authentication;
using Drawer.Application.Services.Authentication.Commands;
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

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            var command = AuthenticationCommands.Register(model.Email, model.Password, model.DisplayName);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailModel model)
        {
            var returnUri = Url.RouteUrl(nameof(VerifyEmailAsync), new { model.RedirectUri }, HttpContext.Request.Scheme)!;
            var command = AuthenticationCommands.ConfirmEmail(model.Email, returnUri);
            var result = await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route("VerifyEmail", Name = nameof(VerifyEmailAsync))]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string email, [FromQuery] string token, [FromQuery] string redirectUri)
        {
            var command = AuthenticationCommands.VerifyEmail(email, token);
            var result = await _mediator.Send(command);
            return Redirect(redirectUri);
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            var command = AuthenticationCommands.Login(model.Email, model.Password);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("Refresh")]
        [ProducesResponseType(typeof(RefreshResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshModel model)
        {
            var command = AuthenticationCommands.Refresh(model.Email, model.RefreshToken);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// API 권한 테스트 용도로만 사용한다
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SecurityTest")]
        [Authorize]
        public IActionResult SecurityTest()
        {
            return Ok("You are authorized");
        }


    }
}
