using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Contract;
using Drawer.Contract.Inventory;
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
        [ProducesResponseType(typeof(GetIssuesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIssues(DateTime? from, DateTime? to)
        {
            List<IssueContracts.Issue> issues;
            if (from.HasValue && to.HasValue)
            {
                var query = new GetIssuesQuery(from.Value, to.Value);
                var result = await _mediator.Send(query) ?? default!;
                issues = result.Issues.Select(x =>
                    new IssueContracts.Issue(x.Id,
                                                x.TransactionNumber,
                                                x.IssueDateTimeLocal,
                                                x.ItemId,
                                                x.LocationId,
                                                x.Quantity,
                                                x.Buyer)).ToList();
            }
            else
            {
                issues = new();
            }
            return Ok(new GetIssuesResponse(issues));
        }


        [HttpGet]
        [Route(ApiRoutes.Issues.Get)]
        [ProducesResponseType(typeof(GetIssueResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIssue([FromRoute] long id)
        {
            var query = new GetIssueQuery(id);
            var result = await _mediator.Send(query) ?? default!;
            var issue = result.Issue;
            if (issue == null)
                return NoContent();

            var issueDto = new IssueContracts.Issue(issue.Id,
                                                    issue.TransactionNumber,
                                                    issue.IssueDateTimeLocal,
                                                    issue.ItemId,
                                                    issue.LocationId,
                                                    issue.Quantity,
                                                    issue.Buyer);

            return Ok(new GetIssueResponse(issueDto));
        }

        [HttpPost]
        [Route(ApiRoutes.Issues.Create)]
        [ProducesResponseType(typeof(CreateIssueResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueRequest request)
        {
            var command = new CreateIssueCommand(request.IssueDateTime, request.ItemId, request.LocationId, request.Quantity, request.Buyer);
            var result = await _mediator.Send(command);
            return Ok(new CreateIssueResponse(result.Id, result.TransactionNumber));
        }

        [HttpPut]
        [Route(ApiRoutes.Issues.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateIssue([FromRoute] long id, [FromBody] UpdateIssueRequest request)
        {
            var command = new UpdateIssueCommand(id, request.IssueDateTime, request.ItemId, request.LocationId, request.Quantity, request.Buyer);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Issues.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteIssue([FromRoute] long id)
        {
            var command = new DeleteIssueCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
