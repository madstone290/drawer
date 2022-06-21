using Drawer.Application.Services.UserInformation;
using Drawer.Contract;
using Drawer.Contract.UserInformation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Drawer.Api.Controllers.UserInformation
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 사용자의 정보를 조회한다.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.User.GetUser)]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser()
        {
            var email = HttpContext.User.Claims.First(x=> x.Type == ClaimTypes.Email).Value;
            var query = UserInformationQueries.GetUser(email);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            return Ok(new GetUserResponse(result.Id, result.Email, result.DisplayName));
        }

        /// <summary>
        /// 사용자의 정보를 수정한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.User.UpdateUser)]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var command = UserInformationCommands.UpdateUser(email, request.DisplayName);
            var result = await _mediator.Send(command);
            return Ok(new UpdateUserResponse(result.DisplayName));
        }

        /// <summary>
        /// 사용자의 비밀번호를 변경한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.User.UpdatePassword)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var command = UserInformationCommands.UpdatePassword(email, request.CurrentPassword, request.NewPassword);
            var result = await _mediator.Send(command);
            return Ok();
        }
    }
}
