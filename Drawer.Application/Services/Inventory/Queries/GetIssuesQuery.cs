using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetIssuesQuery(DateTime From, DateTime To) : IQuery<List<IssueQueryModel>>;

    public class GetIssuesQueryHandler : IQueryHandler<GetIssuesQuery, List<IssueQueryModel>>
    {
        private readonly IIssueRepository _issueRepository;

        public GetIssuesQueryHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<List<IssueQueryModel>> Handle(GetIssuesQuery query, CancellationToken cancellationToken)
        {
            var issues = await _issueRepository.QueryByIssueDateBetween(query.From, query.To);

            return issues;
        }
    }
}
