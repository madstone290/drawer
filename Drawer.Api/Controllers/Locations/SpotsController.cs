using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class SpotsController : ApiController
    {
        private readonly IMediator _mediator;

        public SpotsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Spots.GetList)]
        [ProducesResponseType(typeof(GetSpotsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpots()
        {
            var query = new GetSpotsQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetSpotsResponse(
                    result.Spots.Select(x => new GetSpotsResponse.Spot(x.Id, x.ZoneId, x.Name, x.Note)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Spots.Get)]
        [ProducesResponseType(typeof(GetSpotResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpot([FromRoute] long id)
        {
            var query = new GetSpotQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetSpotResponse(result.Id, result.ZoneId, result.Name, result.Note));
        }

        [HttpPost]
        [Route(ApiRoutes.Spots.Create)]
        [ProducesResponseType(typeof(CreateSpotResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSpot([FromBody] CreateSpotRequest request)
        {
            var command = new CreateSpotCommand(request.ZoneId, request.Name, request.Note);
            var result = await _mediator.Send(command);
            return Ok(new CreateSpotResponse(result.Id));
        }

        [HttpPut]
        [Route(ApiRoutes.Spots.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSpot([FromRoute] long id, [FromBody] UpdateSpotRequest request)
        {
            var command = new UpdateSpotCommand(id, request.Name, request.Note);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Spots.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSpot([FromRoute] long id)
        {
            var command = new DeleteSpotCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
