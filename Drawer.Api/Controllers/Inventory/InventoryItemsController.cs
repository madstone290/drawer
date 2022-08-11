using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;

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
        [ProducesResponseType(typeof(List<InventoryItemQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInventoryDetail([FromRoute] long? itemId, long? locationId)
        {
            var query = new GetInventoryItemsQuery(itemId, locationId);
            var inventoryItem = await _mediator.Send(query);
            return Ok(inventoryItem);
        }

        [HttpPut]
        [Route(ApiRoutes.InventoryItems.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateInventoryItem([FromBody] InventoryItemCommandModel item)
        {
            var command = new InventoryItemUpdateCommand(item);
            var unit = await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.InventoryItems.BatchUpdate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchUpdateInventoryItem([FromBody] List<InventoryItemCommandModel> itemList)
        {
            var command = new InventoryItemBatchUpdateCommand(itemList);
            var result = await _mediator.Send(command);
            return Ok();
        }

     
    }
}
