using Drawer.Api.Controllers;
using Drawer.Application.Authentication;
using Drawer.Application.Services.Authentication.Commands;
using Drawer.Contract.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Authentication
{
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            var command = AuthenticationCommands.RegisterCommand(model.Email, model.Password, model.DisplayName);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginModel), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] LoginModel model)
        {

            return Ok();
        }

    }
}
