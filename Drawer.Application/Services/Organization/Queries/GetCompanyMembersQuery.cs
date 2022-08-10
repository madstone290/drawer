using Drawer.Application.Config;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Application.Services.Organization.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Queries
{
    public record class GetCompanyMembersQuery(string CompanyId) : IQuery<List<CompanyMemberQueryModel>>;

    public class GetCompanyMembersQueryHandler : IQueryHandler<GetCompanyMembersQuery, List<CompanyMemberQueryModel>>
    {
        private readonly ICompanyMemberRepository _companyMemberRepository;

        public GetCompanyMembersQueryHandler(ICompanyMemberRepository companyMemberRepository)
        {
            _companyMemberRepository = companyMemberRepository;
        }

        public async Task<List<CompanyMemberQueryModel>> Handle(GetCompanyMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _companyMemberRepository.QueryByCompanyId(request.CompanyId);

            return members;
        }
    }
}
