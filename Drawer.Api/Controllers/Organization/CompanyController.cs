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
        [Route(ApiRoutes.Company.Add)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCommandModel company)
        {
            var identityUserId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.IdentityUserId).Value;
            var command = new CreateCompanyCommand(identityUserId, company);
            var companyId = await _mediator.Send(command);
            return Ok(companyId);
        }

        // TODO 회사구성원 혹은 소유자만 권한 적용
        [HttpPut]
        [Route(ApiRoutes.Company.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyCommandModel company)
        {
            var companyId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value);
            var command = new UpdateCompanyCommand(companyId, company);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route(ApiRoutes.Company.Get)]
        [ProducesResponseType(typeof(CompanyQueryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompany()
        {
            var companyId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value);
            var query = new GetCompanyByIdQuery(companyId);
            var company = await _mediator.Send(query);
            return Ok(company);
        }

        [HttpGet]
        [Route(ApiRoutes.Company.GetMembers)]
        [ProducesResponseType(typeof(List<CompanyMemberQueryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyMembers()
        {
            var companyId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value);
            var query = new GetCompanyMembersQuery(companyId);
            var members = await _mediator.Send(query);
            return Ok(members);
        }

        [HttpDelete]
        [Route(ApiRoutes.Company.RemoveMemeber)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCompanyMember(MemberCommandModel companyMember)
        {
            var companyId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value);
            var command = new MemberRemoveCommand(companyId, companyMember);
            await _mediator.Send(command);
            return Ok();
        }

    }
}
