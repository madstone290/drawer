using Drawer.Application.Services.Inventory.Commands;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Contract;
using Drawer.Contract.Inventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.InventoryManagement
{
    public class ReceiptsController : ApiController
    {
        private readonly IMediator _mediator;

        public ReceiptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Receipts.GetList)]
        [ProducesResponseType(typeof(GetReceiptsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceipts(DateTime? from, DateTime? to)
        {
            List<GetReceiptsResponse.Receipt> receipts;
            if (from.HasValue && to.HasValue)
            {
                var query = new GetReceiptsQuery(from.Value, to.Value);
                var result = await _mediator.Send(query) ?? default!;
                receipts = result.Receipts.Select(x =>
                    new GetReceiptsResponse.Receipt(x.Id, x.ItemId, x.LocationId, x.Quantity, x.ReceiptTime, x.Seller)).ToList();
            }
            else
            {
                receipts = new();
            }
            return Ok(new GetReceiptsResponse(receipts));
        }


        [HttpGet]
        [Route(ApiRoutes.Receipts.Get)]
        [ProducesResponseType(typeof(GetReceiptResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceipt([FromRoute] long id)
        {
            var query = new GetReceiptQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetReceiptResponse(result.Id, result.ItemId, result.LocationId, result.Quantity, result.ReceiptTime, result.Seller));
        }

        [HttpPost]
        [Route(ApiRoutes.Receipts.Create)]
        [ProducesResponseType(typeof(CreateReceiptResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReceipt([FromBody] CreateReceiptRequest request)
        {
            var command = new CreateReceiptCommand(request.ItemId, request.LocationId, request.Quantity, request.ReceiptTime, request.Seller);
            var result = await _mediator.Send(command);
            return Ok(new CreateReceiptResponse(result.Id));
        }

        [HttpPut]
        [Route(ApiRoutes.Receipts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReceipt([FromRoute] long id, [FromBody] UpdateReceiptRequest request)
        {
            var command = new UpdateReceiptCommand(id, request.ItemId, request.LocationId, request.Quantity, request.ReceiptTime, request.Seller);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Receipts.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteReceipt([FromRoute] long id)
        {
            var command = new DeleteReceiptCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
