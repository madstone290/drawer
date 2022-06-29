using Drawer.Application.Services.Locations.Commands;
using Drawer.Application.Services.Locations.Queries;
using Drawer.Application.Services.Locations.Repos;
using Drawer.Contract;
using Drawer.Contract.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class WorkPlacesController : ApiController
    {
        private readonly IMediator _mediator;

        public WorkPlacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.WorkPlaces.GetWorkPlaces)]
        [ProducesResponseType(typeof(GetWorkPlacesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkPlaces()
        {
            var query = new GetWorkPlacesQuery();
            var result = await _mediator.Send(query);
            return Ok(
                new GetWorkPlacesResponse(
                    result.WorkPlaces.Select(x => new GetWorkPlacesResponse.WorkPlace(x.Id, x.Name, x.Description)).ToList()
                )
            );
        }


        [HttpGet]
        [Route(ApiRoutes.WorkPlaces.GetWorkPlace)]
        [ProducesResponseType(typeof(GetWorkPlaceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWorkPlace([FromRoute] long id)
        {
            var query = new GetWorkPlaceQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetWorkPlaceResponse(result.Id, result.Name, result.Description));
        }

        [HttpPost]
        [Route(ApiRoutes.WorkPlaces.CreateWorkPlace)]
        [ProducesResponseType(typeof(CreateWorkPlaceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWorkPlace([FromBody] CreateWorkPlaceRequest request)
        {
            var command = new CreateWorkPlaceCommand(request.Name, request.Description);
            var result = await _mediator.Send(command);
            return Ok(new CreateWorkPlaceResponse(result.Id, result.Name, result.Description));
        }

        [HttpPut]
        [Route(ApiRoutes.WorkPlaces.UpdateWorkPlace)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateWorkPlace([FromRoute] long id, [FromBody] UpdateWorkPlaceRequest request)
        {
            var command = new UpdateWorkPlaceCommand(id, request.Name, request.Description);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route(ApiRoutes.WorkPlaces.DeleteWorkPlace)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteWorkPlace([FromRoute] long id)
        {
            var command = new DeleteWorkPlaceCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
