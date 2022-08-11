using Drawer.Application.Services.Inventory.QueryModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Shared;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;

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
        [Route(ApiRoutes.Receipts.Add)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddReceipt([FromBody] ReceiptCommandModel receipt)
        {
            var command = new ReceiptAddCommand(receipt);
            var receiptId = await _mediator.Send(command);
            return Ok(receiptId);
        }


        [HttpPost]
        [Route(ApiRoutes.Receipts.BatchAdd)]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchAddReceipt([FromBody] List<ReceiptCommandModel> receiptList)
        {
            var command = new ReceiptBatchAddCommand(receiptList);
            var locationIdList = await _mediator.Send(command);
            return Ok(locationIdList);
        }

        [HttpPut]
        [Route(ApiRoutes.Receipts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReceipt([FromRoute] long id, [FromBody] ReceiptCommandModel receipt)
        {
            var command = new ReceiptUpdateCommand(id, receipt);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Receipts.Remove)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveReceipt([FromRoute] long id)
        {
            var command = new ReceiptRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
