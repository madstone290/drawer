using Drawer.Application.Config;
using Drawer.Application.Services.Organization.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Application.Services.Organization.Queries.GetCompanyMembersResult;

namespace Drawer.Application.Services.Organization.Queries
{
    public record class GetCompanyMembersQuery(string CompanyId) : IQuery<GetCompanyMembersResult>;

    public record GetCompanyMembersResult(IList<Member> Members)
    {
        public record Member(string CompanyId, string UserId);
    };

    public class GetCompanyMembersQueryHandler : IQueryHandler<GetCompanyMembersQuery, GetCompanyMembersResult>
    {
        private readonly ICompanyMemberRepository _companyMemberRepository;

        public GetCompanyMembersQueryHandler(ICompanyMemberRepository companyMemberRepository)
        {
            _companyMemberRepository = companyMemberRepository;
        }

        public async Task<GetCompanyMembersResult> Handle(GetCompanyMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _companyMemberRepository.FindByCompanyIdAsync(request.CompanyId);

            return new GetCompanyMembersResult(members.Select(m => new Member(m.CompanyId, m.UserId)).ToList());
        }
    }
}
