using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class WorkplacesController : ApiController
    {
        private readonly IMediator _mediator;

        public WorkplacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Workplaces.GetList)]
        [ProducesResponseType(typeof(GetWorkplacesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkPlaces()
        {
            var query = new GetWorkPlacesQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetWorkplacesResponse(
                    result.WorkPlaces.Select(x => new GetWorkplacesResponse.Workplace(x.Id, x.Name, x.Note)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.Workplaces.Get)]
        [ProducesResponseType(typeof(GetWorkplaceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkPlace([FromRoute] long id)
        {
            var query = new GetWorkPlaceQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetWorkplaceResponse(result.Id, result.Name, result.Note));
        }

        [HttpPost]
        [Route(ApiRoutes.Workplaces.Create)]
        [ProducesResponseType(typeof(CreateWorkplaceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWorkPlace([FromBody] CreateWorkplaceRequest request)
        {
            var command = new CreateWorkPlaceCommand(request.Name, request.Note);
            var result = await _mediator.Send(command);
            return Ok(new CreateWorkplaceResponse(result.Id));
        }

        [HttpPost]
        [Route(ApiRoutes.Workplaces.BatchCreate)]
        [ProducesResponseType(typeof(BatchCreateWorkplaceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchCreateItem([FromBody] BatchCreateWorkplaceRequest request)
        {
            var command = new BatchCreateWorkplaceCommand(request.Workplaces.Select(x =>
                new BatchCreateWorkplaceCommand.Workplace(x.Name, x.Note))
                .ToList());
            var result = await _mediator.Send(command);
            return Ok(new BatchCreateWorkplaceResponse(result.IdList));
        }

        [HttpPut]
        [Route(ApiRoutes.Workplaces.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateWorkPlace([FromRoute] long id, [FromBody] UpdateWorkplaceRequest request)
        {
            var command = new UpdateWorkPlaceCommand(id, request.Name, request.Note);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.Workplaces.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteWorkPlace([FromRoute] long id)
        {
            var command = new DeleteWorkPlaceCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
