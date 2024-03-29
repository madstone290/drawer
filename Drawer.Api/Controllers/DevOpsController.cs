﻿using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers
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
        [Route("DeploymentVersion")]
        public string GetVersion()
        {
            return VERSION;
        }
    }
}
