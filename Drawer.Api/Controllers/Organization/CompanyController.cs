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

        /// <summary>
        /// 사용자의 정보를 수정한다.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
