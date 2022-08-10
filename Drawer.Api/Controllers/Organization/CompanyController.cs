using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Commands;
using Drawer.Application.Services.Organization.Queries;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drawer.Api.Controllers.Organization
{
    public class CompanyController : ApiController
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.Company.Create)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyAddUpdateCommandModel company)
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.UserId).Value;
            var command = new CreateCompanyCommand(userId, company);
            var companyId = await _mediator.Send(command);
            return Ok(companyId);
        }

        // TODO 회사구성원 혹은 소유자만 권한 적용
        [HttpPut]
        [Route(ApiRoutes.Company.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyAddUpdateCommandModel company)
        {
            var companyId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.CompanyId).Value;
            var command = new UpdateCompanyCommand(companyId, company);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route(ApiRoutes.Company.Get)]
        [ProducesResponseType(typeof(CompanyQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompany()
        {
            var companyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value;
            if (companyId == null)
                return Ok();

            var query = new GetCompanyByIdQuery(companyId);
            var company = await _mediator.Send(query);
            return Ok(company);
        }

        [HttpGet]
        [Route(ApiRoutes.Company.GetMembers)]
        [ProducesResponseType(typeof(List<CompanyMemberQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyMembers()
        {
            var companyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value;
            if (companyId == null)
                return NoContent();
            var query = new GetCompanyMembersQuery(companyId);
            var members = await _mediator.Send(query);
            return Ok(members);
        }
    }
}
