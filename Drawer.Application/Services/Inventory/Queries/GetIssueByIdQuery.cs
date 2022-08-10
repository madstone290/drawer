using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetIssueByIdQuery(long Id) : IQuery<IssueQueryModel?>;

    public class GetIssueQueryHandler : IQueryHandler<GetIssueByIdQuery, IssueQueryModel?>
    {
        private readonly IIssueRepository _issueRepository;

        public GetIssueQueryHandler(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task<IssueQueryModel?> Handle(GetIssueByIdQuery query, CancellationToken cancellationToken)
        {
            var issue = await _issueRepository.QueryById(query.Id);
            return issue;
        }
    }
}
