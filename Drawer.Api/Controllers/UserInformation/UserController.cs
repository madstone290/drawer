using Drawer.Application.Services.UserInformation.CommandModels;
using Drawer.Application.Services.UserInformation.Commands;
using Drawer.Application.Services.UserInformation.Queries;
using Drawer.Application.Services.UserInformation.QueryModels;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Route(ApiRoutes.User.Get)]
        [ProducesResponseType(typeof(UserQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.User.Claims.First(x=> x.Type == DrawerClaimTypes.IdentityUserId).Value;
            var query = new GetUserQuery(userId);
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        /// <summary>
        /// 사용자의 정보를 수정한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.User.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UserCommandModel userInfo)
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.IdentityUserId).Value;
            var command = new UpdateUserInfoCommand(userId, userInfo);
            var unit = await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// 사용자의 비밀번호를 변경한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(ApiRoutes.User.UpdatePassword)]
        public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordCommandModel userPassword)
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.IdentityUserId).Value;
            var command = new UpdatePasswordCommand(userId, userPassword);
            var unit = await _mediator.Send(command);
            return Ok();
        }
    }
}
