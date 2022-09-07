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
    public record GetCompanyJoinRequestsByCompanyIdQuery(long CompanyId) : IQuery<List<CompanyJoinRequestQueryModel>>;

    public class GetCompanyJoinRequestsQueryHandler : IQueryHandler<GetCompanyJoinRequestsByCompanyIdQuery, List<CompanyJoinRequestQueryModel>>
    {
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;

        public GetCompanyJoinRequestsQueryHandler(ICompanyJoinRequestRepository joinRequestRepository)
        {
            _joinRequestRepository = joinRequestRepository;
        }

        public async Task<List<CompanyJoinRequestQueryModel>> Handle(GetCompanyJoinRequestsByCompanyIdQuery query, CancellationToken cancellationToken)
        {
            var requestList = await _joinRequestRepository.QueryByCompanyId(query.CompanyId);

            return requestList;
        }
    }
}
