using Drawer.Application.Services.InventoryManagement.Commands;
using Drawer.Application.Services.InventoryManagement.Queries;
using Drawer.Contract;
using Drawer.Contract.InventoryManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.BasicInfo
{
    public class InventoryController : ApiController
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Inventory.Get)]
        [ProducesResponseType(typeof(GetInventoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInventoryDetail([FromRoute] long? itemId, long? locationId)
        {
            var query = new GetInventoryQuery(itemId, locationId);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetInventoryResponse(result.InventoryDetails.Select(x=> 
                    new GetInventoryResponse.InventoryDetail(x.ItemId, x.LocationId, x.Quantity)).ToList()));
        }

        [HttpPut]
        [Route(ApiRoutes.Inventory.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateInventoryDetail([FromBody] UpdateInventoryRequest request)
        {
            var command = new UpdateInventoryCommand(request.ItemId, request.LocationId, request.QuantityChange);
            var result = await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.Inventory.BatchUpdate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchUpdateInventory([FromBody] BatchUpdateInventoryRequest request)
        {
            var command = new BatchUpdateInventoryCommand(request.Changes.Select(x =>
                new BatchUpdateInventoryCommand.InventoryChange(x.ItemId, x.LocationId, x.QuantityChange))
                .ToList());
            var result = await _mediator.Send(command);
            return Ok();
        }

     
    }
}
