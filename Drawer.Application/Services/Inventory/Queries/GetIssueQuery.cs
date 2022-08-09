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
    public record GetIssueQuery(long Id) : IQuery<GetIssueResult?>;

    public record GetIssueResult(Issue? Issue);

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
            return new GetIssueResult(issue);
        }
    }
}
