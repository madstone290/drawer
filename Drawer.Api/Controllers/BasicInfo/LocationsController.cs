using Drawer.Application.Services.BasicInfo.Commands;
using Drawer.Application.Services.BasicInfo.Queries;
using Drawer.Contract;
using Drawer.Contract.BasicInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.BasicInfo
{
    public class LocationsController : ApiController
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Locations.GetList)]
        [ProducesResponseType(typeof(GetLocationsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocations()
        {
            var query = new GetLocationsQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetLocationsResponse(
                    result.Locations.Select(x => new GetLocationsResponse.Location(x.Id, x.UpperLocationId, x.Name, x.Note)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Locations.Get)]
        [ProducesResponseType(typeof(GetLocationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocation([FromRoute] long id)
        {
            var query = new GetLocationQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetLocationResponse(result.Id, result.UpperLocationId, result.Name, result.Note));
        }

        [HttpPost]
        [Route(ApiRoutes.Locations.Create)]
        [ProducesResponseType(typeof(CreateLocationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationRequest request)
        {
            var command = new CreateLocationCommand(request.UpperLocationId, request.Name, request.Note);
            var result = await _mediator.Send(command);
            return Ok(new CreateLocationResponse(result.Id));
        }

        [HttpPost]
        [Route(ApiRoutes.Locations.BatchCreate)]
        [ProducesResponseType(typeof(BatchCreateLocationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchCreateItem([FromBody] BatchCreateLocationRequest request)
        {
            var command = new BatchCreateLocationCommand(request.Locations.Select(x =>
                new BatchCreateLocationCommand.Location(x.UpperLocationId, x.Name, x.Note))
                .ToList());
            var result = await _mediator.Send(command);
            return Ok(new BatchCreateLocationResponse(result.IdList));
        }

        [HttpPut]
        [Route(ApiRoutes.Locations.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLocation([FromRoute] long id, [FromBody] UpdateLocationRequest request)
        {
            var command = new UpdateLocationCommand(id, request.Name, request.Note);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Locations.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLocation([FromRoute] long id)
        {
            var command = new DeleteLocationCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
