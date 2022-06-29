using Drawer.Application.Services.Items.Commands;
using Drawer.Application.Services.Items.Queries;
using Drawer.Contract;
using Drawer.Contract.Items;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Items
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
        [ProducesResponseType(typeof(GetItemsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems()
        {
            var query = new GetItemsQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetItemsResponse(
                    result.Items.Select(x => 
                    new GetItemsResponse.Item(x.Id, x.Name, x.Code, x.Number, x.Sku, x.MeasurementUnit)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Items.Get)]
        [ProducesResponseType(typeof(GetItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItem([FromRoute] long id)
        {
            var query = new GetItemQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetItemResponse(result.Id, result.Name, result.Code, 
                    result.Number, result.Sku, result.MeasurementUnit));
        }

        [HttpPost]
        [Route(ApiRoutes.Items.Create)]
        [ProducesResponseType(typeof(CreateItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
        {
            var command = new CreateItemCommand(request.Name, request.Code,
                request.Number, request.Sku, request.MeasurementUnit);
            var result = await _mediator.Send(command);
            return Ok(new CreateItemResponse(result.Id));
        }

        [HttpPut]
        [Route(ApiRoutes.Items.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateItem([FromRoute] long id, [FromBody] UpdateItemRequest request)
        {
            var command = new UpdateItemCommand(id, request.Name, request.Code,
                request.Number, request.Sku, request.MeasurementUnit);
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
