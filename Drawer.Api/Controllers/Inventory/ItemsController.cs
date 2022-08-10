using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.Commands.ItemCommands;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;

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
        public async Task<IActionResult> CreateItem([FromBody] ItemAddUpdateCommandModel item)
        {
            var command = new CreateItemCommand(item);
            var itemId = await _mediator.Send(command);
            return Ok(itemId);
        }


        [HttpPost]
        [Route(ApiRoutes.Items.BatchCreate)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchCreateItem([FromBody] List<ItemAddUpdateCommandModel> itemList)
        {
            var command = new BatchCreateItemCommand(itemList);
            var itemIdList = await _mediator.Send(command);
            return Ok(itemIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Items.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateItem([FromRoute] long id, [FromBody] ItemAddUpdateCommandModel item)
        {
            var command = new UpdateItemCommand(id, item);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Items.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteItem([FromRoute] long id)
        {
            var command = new DeleteItemCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
