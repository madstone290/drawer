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
            List<ReceiptContracts.Receipt> receipts;
            if (from.HasValue && to.HasValue)
            {
                var query = new GetReceiptsQuery(from.Value, to.Value);
                var result = await _mediator.Send(query) ?? default!;
                receipts = result.Receipts.Select(x =>
                    new ReceiptContracts.Receipt(x.Id,
                                                 x.TransactionNumber,
                                                 x.ReceiptDateTimeLocal,
                                                 x.ItemId,
                                                 x.LocationId,
                                                 x.Quantity,
                                                 x.Seller)).ToList();
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
            var result = await _mediator.Send(query) ?? default!;
            var receipt = result.Receipt;
            if (receipt == null)
                return NoContent();


            var receiptDto = new ReceiptContracts.Receipt(receipt.Id,
                                                         receipt.TransactionNumber,
                                                         receipt.ReceiptDateTimeLocal,
                                                         receipt.ItemId,
                                                         receipt.LocationId,
                                                         receipt.Quantity,
                                                         receipt.Seller);
            return Ok(new GetReceiptResponse(receiptDto));
        }

        [HttpPost]
        [Route(ApiRoutes.Receipts.Create)]
        [ProducesResponseType(typeof(CreateReceiptResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReceipt([FromBody] CreateReceiptRequest request)
        {
            var command = new CreateReceiptCommand(request.ReceiptDateTime, request.ItemId, request.LocationId, request.Quantity, request.Seller);
            var result = await _mediator.Send(command);
            return Ok(new CreateReceiptResponse(result.Id, result.TransactionNumber));
        }

        [HttpPut]
        [Route(ApiRoutes.Receipts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReceipt([FromRoute] long id, [FromBody] UpdateReceiptRequest request)
        {
            var command = new UpdateReceiptCommand(id, request.ReceiptDateTime, request.ItemId, request.LocationId, request.Quantity, request.Seller);
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
