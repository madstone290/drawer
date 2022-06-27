using Drawer.Application.Services.Locations.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Locations
{
    public class WorkPlacesController : ApiController
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public WorkPlacesController(IWorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        [HttpGet]
        [Route("api/fdfdf")]
        public async Task<IActionResult> TestAsync([FromQuery] string value)
        {
            var obj = await _workPlaceRepository.FilterTest(value);
            return Ok();
        }
    }
}
