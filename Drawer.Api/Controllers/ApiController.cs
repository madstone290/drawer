using Drawer.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Drawer.Api.Controllers
{
    [ServiceFilter(typeof(DefaultExceptionFilter))]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ApiController : ControllerBase
    {
    }
}
