using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;

namespace Drawer.Api.Controllers.InventoryManagement
{
    public class ItemsController : ApiController
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Items.GetList)]
        [ProducesResponseType(typeof(List<ItemQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems()
        {
            var query = new GetItemsQuery();
            var itemList = await _mediator.Send(query);
            return Ok(itemList);
        }


        [HttpGet]
        [Route(ApiRoutes.Items.Get)]
        [ProducesResponseType(typeof(ItemQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItem([FromRoute] long id)
        {
            var query = new GetItemByIdQuery(id);
            var item = await _mediator.Send(query);
            return Ok(item);
        }

        [HttpPost]
        [Route(ApiRoutes.Items.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddItem([FromBody] ItemCommandModel item)
        {
            var command = new ItemAddCommand(item);
            var itemId = await _mediator.Send(command);
            return Ok(itemId);
        }


        [HttpPost]
        [Route(ApiRoutes.Items.BatchCreate)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchAddItem([FromBody] List<ItemCommandModel> itemList)
        {
            var command = new ItemBatchAddCommand(itemList);
            var itemIdList = await _mediator.Send(command);
            return Ok(itemIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Items.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateItem([FromRoute] long id, [FromBody] ItemCommandModel item)
        {
            var command = new ItemUpdateCommand(id, item);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Items.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItem([FromRoute] long id)
        {
            var command = new ItemRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
