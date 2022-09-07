using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Commands;
using Drawer.Application.Services.Organization.Queries;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Organization
{
    public class CompanyJoinRequestsController : ApiController
    {
        private readonly IMediator _mediator;

        public CompanyJoinRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.JoinRequests.Add)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddRequest([FromBody] JoinRequestAddCommandModel request)
        {
            var userId  = Convert.ToInt64(HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.UserId).Value);
            var command = new JoinRequestAddCommand(userId, request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.JoinRequests.Handle)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRequest([FromRoute] long id, [FromBody] JoinRequestHandleCommandModel joinRequest)
        {
            var command = new JoinRequestHandleCommand(id, joinRequest);
            await _mediator.Send(command);
            return Ok();
        }


        [HttpGet]
        [Route(ApiRoutes.JoinRequests.GetList)]
        [ProducesResponseType(typeof(List<CompanyJoinRequestQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequests()
        {
            var companyId = Convert.ToInt64(HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.CompanyId).Value);
            var query = new GetCompanyJoinRequestsByCompanyIdQuery(companyId);
            var requestList = await _mediator.Send(query);
            return Ok(requestList);
        }
    }
}
