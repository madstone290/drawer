using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using Drawer.Domain.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetIssuesQuery(DateTime From, DateTime To) : IQuery<GetIssuesResult?>;

    public record GetIssuesResult(List<Issue> Issues);

    public class GetIssuesQueryHandler : IQueryHandler<GetIssuesQuery, GetIssuesResult?>
    {
        private readonly IIssueRepository _issueRepository;

        public GetIssuesQueryHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<GetIssuesResult?> Handle(GetIssuesQuery query, CancellationToken cancellationToken)
        {
            var issues = await _issueRepository.FindByIssueDateBetween(query.From, query.To);

            return new GetIssuesResult(issues);
        }
    }
}
