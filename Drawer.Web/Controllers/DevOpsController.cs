using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Drawer.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevOpsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private const string VERSION = "1.0.1";

        public DevOpsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("DeploymentColor")]
        public string GetDeploymentColor()
        {
            return _configuration.GetValue<string>("DeploymentColor") ?? "n/a";
        }

        [HttpGet]
        [Route("Version")]
        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }
    }
}
