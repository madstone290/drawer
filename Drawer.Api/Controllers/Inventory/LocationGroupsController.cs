using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;

namespace Drawer.Api.Controllers.InventoryManagement
{
    public class LocationGroupsController : ApiController
    {
        private readonly IMediator _mediator;

        public LocationGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.LocationGroups.GetList)]
        [ProducesResponseType(typeof(List<LocationGroupQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocationGroups()
        {
            var query = new GetLocationGroupsQuery();
            var locationList = await _mediator.Send(query);
            return Ok(locationList);
        }


        [HttpGet]
        [Route(ApiRoutes.LocationGroups.Get)]
        [ProducesResponseType(typeof(LocationGroupQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocationGroup([FromRoute] long id)
        {
            var query = new GetLocationGroupByIdQuery(id);
            var location = await _mediator.Send(query);
            return Ok(location);
        }

        [HttpPost]
        [Route(ApiRoutes.LocationGroups.Add)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] LocationGroupAddCommandModel location)
        {
            var command = new LocationGroupAddCommand(location);
            var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [HttpPost]
        [Route(ApiRoutes.LocationGroups.BatchAdd)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchAdd([FromBody] List<LocationGroupAddCommandModel> locationList)
        {
            var command = new LocationGroupBatchAddCommand(locationList);
            var locationIdList = await _mediator.Send(command);
            return Ok(locationIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.LocationGroups.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] LocationGroupUpdateCommandModel location)
        {
            var command = new LocationGroupUpdateCommand(id, location);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.LocationGroups.Remove)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            var command = new LocationGroupRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
