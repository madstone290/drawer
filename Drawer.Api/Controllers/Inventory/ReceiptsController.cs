using Drawer.Application.Services.Inventory.QueryModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Shared;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.Commands.ReceiptCommands;
using Drawer.Application.Services.Inventory.CommandModels;

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
        [ProducesResponseType(typeof(List<ReceiptQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceipts([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            List<ReceiptQueryModel> receipts;
            if (from.HasValue && to.HasValue)
            {
                var query = new GetReceiptsByDateQuery()
                {
                    From = from.Value,
                    To = to.Value
                };
                var result = await _mediator.Send(query) ?? default!;
                receipts = result;
            }
            else
            {
                receipts = new();
            }
            return Ok(receipts);
        }


        [HttpGet]
        [Route(ApiRoutes.Receipts.Get)]
        [ProducesResponseType(typeof(ReceiptQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceipt([FromRoute] long id)
        {
            var query = new GetReceiptByIdQuery(id);
            var receipt = await _mediator.Send(query);
            return Ok(receipt);
        }

        [HttpPost]
        [Route(ApiRoutes.Receipts.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReceipt([FromBody] ReceiptAddUpdateCommandModel receipt)
        {
            var command = new CreateReceiptCommand(receipt);
            var receiptId = await _mediator.Send(command);
            return Ok(receiptId);
        }

        [HttpPut]
        [Route(ApiRoutes.Receipts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReceipt([FromRoute] long id, [FromBody] ReceiptAddUpdateCommandModel receipt)
        {
            var command = new UpdateReceiptCommand(id, receipt);
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
