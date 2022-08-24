﻿using Drawer.Shared;
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
            var locationList = await _mediator.Send(query);
            return Ok(locationList);
        }


        [HttpGet]
        [Route(ApiRoutes.Layouts.Get)]
        [ProducesResponseType(typeof(LayoutQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLayout([FromRoute] long id)
        {
            var query = new GetLayoutByIdQuery(id);
            var location = await _mediator.Send(query);
            return Ok(location);
        }

        [HttpPost]
        [Route(ApiRoutes.Layouts.Add)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] LayoutAddCommandModel location)
        {
            var command = new LayoutAddCommand(location);
            var locationId = await _mediator.Send(command);
            return Ok(locationId);
        }

        [HttpPut]
        [Route(ApiRoutes.Layouts.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] LayoutUpdateCommandModel location)
        {
            var command = new LayoutUpdateCommand(id, location);
            await _mediator.Send(command);
            return Ok();
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
