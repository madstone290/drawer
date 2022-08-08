using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetIssueQuery(long Id) : IQuery<GetIssueResult?>;

    public record GetIssueResult(long Id, long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer);

    public class GetIssueQueryHandler : IQueryHandler<GetIssueQuery, GetIssueResult?>
    {
        private readonly IIssueRepository _issueRepository;

        public GetIssueQueryHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<GetIssueResult?> Handle(GetIssueQuery query, CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.FindByIdAsync(query.Id);

            return issue == null
                ? null
                : new GetIssueResult(issue.Id, issue.ItemId, issue.LocationId, issue.Quantity,
                    issue.IssueTimeLocal, issue.Buyer);
        }
    }
}
