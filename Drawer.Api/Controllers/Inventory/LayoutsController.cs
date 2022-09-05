using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Drawer.Application.Services.Inventory.Queries;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.Commands;

namespace Drawer.Api.Controllers.InventoryManagement
{
    public class LayoutsController : ApiController
    {
        private readonly IMediator _mediator;

        public LayoutsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Layouts.GetList)]
        [ProducesResponseType(typeof(List<LayoutQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLayouts()
        {
            var query = new GetLayoutsQuery();
            var layoutList = await _mediator.Send(query);
            return Ok(layoutList);
        }


        [HttpGet]
        [Route(ApiRoutes.Layouts.Get)]
        [ProducesResponseType(typeof(LayoutQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLayout([FromRoute] long id)
        {
            var query = new GetLayoutByIdQuery(id);
            var layout = await _mediator.Send(query);
            return Ok(layout);
        }

        [HttpGet]
        [Route(ApiRoutes.Layouts.GetByLocationGroup)]
        [ProducesResponseType(typeof(LayoutQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLayoutByLocation([FromRoute] long groupId)
        {
            var query = new GetLayoutByLocationQuery(groupId);
            var layout = await _mediator.Send(query);
            return Ok(layout);
        }


        [HttpPost]
        [Route(ApiRoutes.Layouts.Edit)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] LayoutEditCommandModel layout)
        {
            var command = new LayoutEditCommand(layout);
            var layoutId = await _mediator.Send(command);
            return Ok(layoutId);
        }

        [HttpDelete]
        [Route(ApiRoutes.Layouts.Remove)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            var command = new LayoutRemoveCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
