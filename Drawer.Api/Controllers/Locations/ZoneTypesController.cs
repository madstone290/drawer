using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class ZoneTypesController : ApiController
    {
        private readonly IMediator _mediator;

        public ZoneTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.ZoneTypes.GetList)]
        [ProducesResponseType(typeof(GetZoneTypesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetZoneTypes()
        {
            var query = new GetZoneTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetZoneTypesResponse(
                    result.ZoneTypes.Select(x => new GetZoneTypesResponse.ZoneType(x.Id, x.Name)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.ZoneTypes.Get)]
        [ProducesResponseType(typeof(GetZoneTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetZoneType([FromRoute] long id)
        {
            var query = new GetZoneTypeQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetZoneTypeResponse(result.Id, result.Name));
        }

        [HttpPost]
        [Route(ApiRoutes.ZoneTypes.Create)]
        [ProducesResponseType(typeof(CreateZoneTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateZoneType([FromBody] CreateZoneTypeRequest request)
        {
            var command = new CreateZoneTypeCommand(request.Name);
            var result = await _mediator.Send(command);
            return Ok(new CreateZoneTypeResponse(result.Id, result.Name));
        }

        [HttpPut]
        [Route(ApiRoutes.ZoneTypes.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateZoneType([FromRoute] long id, [FromBody] UpdateZoneTypeRequest request)
        {
            var command = new UpdateZoneTypeCommand(id, request.Name);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.ZoneTypes.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteZoneType([FromRoute] long id)
        {
            var command = new DeleteZoneTypeCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
