using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.InventoryManagement
{
    public class IssuesController : ApiController
    {
        private readonly IMediator _mediator;

        public IssuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Issues.GetList)]
        [ProducesResponseType(typeof(List<IssueQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIssues([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            List<IssueQueryModel> issues;
            if (from.HasValue && to.HasValue)
            {
                var query = new GetIssuesQuery(from.Value, to.Value);
                issues = await _mediator.Send(query);
            }
            else
            {
                issues = new();
            }
            return Ok(issues);
        }


        [HttpGet]
        [Route(ApiRoutes.Issues.Get)]
        [ProducesResponseType(typeof(IssueQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIssue([FromRoute] long id)
        {
            var query = new GetIssueByIdQuery(id);
            var issue = await _mediator.Send(query) ?? default!;
            return Ok(issue);
        }

        [HttpPost]
        [Route(ApiRoutes.Issues.Add)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddIssue([FromBody] IssueCommandModel issue)
        {
            var command = new IssueAddCommand(issue);
            var issueId = await _mediator.Send(command);
            return Ok(issueId);
        }

        [HttpPost]
        [Route(ApiRoutes.Issues.BatchAdd)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchAddIssue([FromBody] List<IssueCommandModel> issueList)
        {
            var command = new IssueBatchAddCommand(issueList);
            var locationIdList = await _mediator.Send(command);
            return Ok(locationIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Issues.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateIssue([FromRoute] long id, [FromBody] IssueCommandModel issue)
        {
            var command = new IssueUpdateCommand(id, issue);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Issues.Remove)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveIssue([FromRoute] long id)
        {
            var command = new IssueRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
