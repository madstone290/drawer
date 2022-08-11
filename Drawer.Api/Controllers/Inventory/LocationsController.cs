using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;

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
        public async Task<IActionResult> Add([FromBody] LocationAddCommandModel location)
        {
            var command = new LocationAddCommand(location);
            var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [HttpPost]
        [Route(ApiRoutes.Locations.BatchCreate)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchAdd([FromBody] List<LocationAddCommandModel> locationList)
        {
            var command = new LocationBatchAddCommand(locationList);
            var locationIdList = await _mediator.Send(command);
            return Ok(locationIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Locations.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] LocationUpdateCommandModel location)
        {
            var command = new LocationUpdateCommand(id, location);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Locations.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            var command = new LocationRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
