using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.Commands.LocationCommands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;

namespace Drawer.Api.Controllers.InventoryManagement
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
        [ProducesResponseType(typeof(List<LocationQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocations()
        {
            var query = new GetLocationsQuery();
            var locationList = await _mediator.Send(query);
            return Ok(locationList);
        }


        [HttpGet]
        [Route(ApiRoutes.Locations.Get)]
        [ProducesResponseType(typeof(LocationQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocation([FromRoute] long id)
        {
            var query = new GetLocationByIdQuery(id);
            var location = await _mediator.Send(query);
            return Ok(location);
        }

        [HttpPost]
        [Route(ApiRoutes.Locations.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateLocation([FromBody] LocationAddCommandModel location)
        {
            var command = new CreateLocationCommand(location);
            var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [HttpPost]
        [Route(ApiRoutes.Locations.BatchCreate)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchCreateItem([FromBody] List<LocationAddCommandModel> locationList)
        {
            var command = new BatchCreateLocationCommand(locationList);
            var locationIdList = await _mediator.Send(command);
            return Ok(locationIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Locations.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateLocation([FromRoute] long id, [FromBody] LocationUpdateCommandModel location)
        {
            var command = new UpdateLocationCommand(id, location);
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
