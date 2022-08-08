using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class IssueContracts
    {
    }

    public record CreateIssueRequest(long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer);

    public record CreateIssueResponse(long Id);

    public record UpdateIssueRequest(long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer);

    public record GetIssueResponse(long Id, long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer);

    public record GetIssuesResponse(List<GetIssuesResponse.Issue> Issues)
    {
        public record Issue(long Id, long ItemId, long LocationId, decimal Quantity, DateTime IssueTime, string? Buyer);
    }
}
