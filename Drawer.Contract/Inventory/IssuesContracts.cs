using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Inventory
{
    public class IssueContracts
    {
        public record Issue(long Id, string TransactionNumber, DateTime IssueDateTime, long ItemId, long LocationId, decimal Quantity, string? Buyer);
    }

    public record CreateIssueRequest(DateTime IssueDateTime, long ItemId, long LocationId, decimal Quantity, string? Buyer);

    public record CreateIssueResponse(long Id, string TransactionNumber);

    public record UpdateIssueRequest(DateTime IssueDateTime, long ItemId, long LocationId, decimal Quantity, string? Buyer);

    public record GetIssueResponse(IssueContracts.Issue Issue);

    public record GetIssuesResponse(List<IssueContracts.Issue> Issues);
}
