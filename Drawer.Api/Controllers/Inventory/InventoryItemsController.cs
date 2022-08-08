using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Contract;
using Drawer.Contract.Inventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.BasicInfo
{
    public class InventoryItemsController : ApiController
    {
        private readonly IMediator _mediator;

        public InventoryItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.InventoryItems.Get)]
        [ProducesResponseType(typeof(GetInventoryItemsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInventoryDetail([FromRoute] long? itemId, long? locationId)
        {
            var query = new GetInventoryItemsQuery(itemId, locationId);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetInventoryItemsResponse(result.InventoryItems.Select(x=> 
                    new GetInventoryItemsResponse.InventoryItem(x.ItemId, x.LocationId, x.Quantity)).ToList()));
        }

        [HttpPut]
        [Route(ApiRoutes.InventoryItems.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateInventoryDetail([FromBody] UpdateInventoryRequest request)
        {
            var command = new UpdateInventoryCommand(request.ItemId, request.LocationId, request.QuantityChange);
            var result = await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.InventoryItems.BatchUpdate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchUpdateInventory([FromBody] BatchUpdateInventoryItemRequest request)
        {
            var command = new BatchUpdateInventoryCommand(request.InventoryItemChanges.Select(x =>
                new BatchUpdateInventoryCommand.InventoryChange(x.ItemId, x.LocationId, x.QuantityChange))
                .ToList());
            var result = await _mediator.Send(command);
            return Ok();
        }

     
    }
}
