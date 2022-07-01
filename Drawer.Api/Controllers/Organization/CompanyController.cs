using Drawer.Application.Services.Organization.Commands;
using Drawer.Application.Services.Organization.Queries;
using Drawer.Contract;
using Drawer.Contract.Organization;
using Drawer.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [ProducesResponseType(typeof(CreateCompanyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.UserId).Value;
            var command = new CreateCompanyCommand(userId, request.Name, request.PhoneNumber);
            var result = await _mediator.Send(command);
            return Ok(new CreateCompanyResponse(result.Id, result.OwnerId, result.Name, result.PhoneNumber));
        }

        // TODO 회사구성원 혹은 소유자만 권한 적용
        [HttpPut]
        [Route(ApiRoutes.Company.Update)]
        [ProducesResponseType(typeof(CreateCompanyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request)
        {
            var companyId = HttpContext.User.Claims.First(x => x.Type == DrawerClaimTypes.CompanyId).Value;
            var command = new UpdateCompanyCommand(companyId, request.Name, request.PhoneNumber);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route(ApiRoutes.Company.Get)]
        [ProducesResponseType(typeof(GetCompanyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompany()
        {
            var companyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value;
            if (companyId == null)
                return NoContent();
            var query = new GetCompanyQuery(companyId);
            var result = await _mediator.Send(query);
            if (result == null)
                return NoContent();
            else
                return Ok(new GetCompanyResponse(result.Id, result.OwnerId, result.Name, result.PhoneNumber));
        }

        [HttpGet]
        [Route(ApiRoutes.Company.GetMembers)]
        [ProducesResponseType(typeof(GetCompanyMembersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyMembers()
        {
            var companyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)?.Value;
            if (companyId == null)
                return NoContent();
            var query = new GetCompanyMembersQuery(companyId);
            var result = await _mediator.Send(query);
            return Ok(
                new GetCompanyMembersResponse(result.Members.Select(m => 
                    new GetCompanyMembersResponse.Member(m.CompanyId, m.UserId)).ToList()));
        }
    }
}
