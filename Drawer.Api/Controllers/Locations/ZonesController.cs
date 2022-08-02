using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class ZonesController : ApiController
    {
        private readonly IMediator _mediator;

        public ZonesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Zones.GetList)]
        [ProducesResponseType(typeof(GetZonesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetZones()
        {
            var query = new GetZonesQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetZonesResponse(
                    result.Zones.Select(x => new GetZonesResponse.Zone(x.Id, x.WorkPlaceId, x.Name, x.Note)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Zones.Get)]
        [ProducesResponseType(typeof(GetZoneResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetZone([FromRoute] long id)
        {
            var query = new GetZoneQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetZoneResponse(result.Id, result.WorkPlaceId, result.Name, result.Note));
        }

        [HttpPost]
        [Route(ApiRoutes.Zones.Create)]
        [ProducesResponseType(typeof(CreateZoneResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneRequest request)
        {
            var command = new CreateZoneCommand(request.WorkplaceId, request.Name, request.Note);
            var result = await _mediator.Send(command);
            return Ok(new CreateZoneResponse(result.Id));
        }

        [HttpPost]
        [Route(ApiRoutes.Zones.BatchCreate)]
        [ProducesResponseType(typeof(BatchCreateZoneResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchCreateItem([FromBody] BatchCreateZoneRequest request)
        {
            var command = new BatchCreateZoneCommand(request.Zones.Select(x =>
                new BatchCreateZoneCommand.Zone(x.WorkplaceId, x.Name, x.Note))
                .ToList());
            var result = await _mediator.Send(command);
            return Ok(new BatchCreateZoneResponse(result.IdList));
        }

        [HttpPut]
        [Route(ApiRoutes.Zones.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateZone([FromRoute] long id, [FromBody] UpdateZoneRequest request)
        {
            var command = new UpdateZoneCommand(id, request.Name, request.Note);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Zones.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteZone([FromRoute] long id)
        {
            var command = new DeleteZoneCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
