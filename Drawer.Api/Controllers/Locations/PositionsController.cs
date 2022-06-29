using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class PositionsController : ApiController
    {
        private readonly IMediator _mediator;

        public PositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Positions.GetList)]
        [ProducesResponseType(typeof(GetPositionsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPositions()
        {
            var query = new GetPositionsQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetPositionsResponse(
                    result.Positions.Select(x => new GetPositionsResponse.Position(x.Id, x.Name)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Positions.Get)]
        [ProducesResponseType(typeof(GetPositionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPosition([FromRoute] long id)
        {
            var query = new GetPositionQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetPositionResponse(result.Id, result.Name));
        }

        [HttpPost]
        [Route(ApiRoutes.Positions.Create)]
        [ProducesResponseType(typeof(CreatePositionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequest request)
        {
            var command = new CreatePositionCommand(request.ZoneId, request.Name);
            var result = await _mediator.Send(command);
            return Ok(new CreatePositionResponse(result.Id, result.ZoneId, result.Name));
        }

        [HttpPut]
        [Route(ApiRoutes.Positions.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePosition([FromRoute] long id, [FromBody] UpdatePositionRequest request)
        {
            var command = new UpdatePositionCommand(id, request.Name);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Positions.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePosition([FromRoute] long id)
        {
            var command = new DeletePositionCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
