using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Organization
{
    public partial class CompanyMemberRequest
    {
        private readonly List<JoinRequestModel> _requestList = new();

        private HashSet<JoinRequestModel>? _selectedRequests;

        public int RequestCount => _requestList.Count;

        [Inject]
        public CompanyJoinRequestApiClient JoinRequestApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            await Load_Click();
        }

        async Task Load_Click()
        {
            var response = await JoinRequestApiClient.GetRequests();
            if (!Snackbar.CheckFail(response))
                return;

            _requestList.Clear();
            _requestList.AddRange(response.Data.Where(x=> x.IsHandled == false)
                .Select(x => new JoinRequestModel()
                {
                    Id = x.Id,
                    UserEmail = x.UserEmail,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    RequestTimeLocal = x.RequestTimeUtc.ToLocalTime(),
                }));
        }

        async Task Accept_ClickAsync()
        {
            if (_selectedRequests == null || _selectedRequests.Count == 0)
                return;

            foreach(var request in _selectedRequests)
            {
                var response = await JoinRequestApiClient.HandleRequest(request.Id, new JoinRequestHandleCommandModel()
                {
                    IsAccepted = true
                });

                Snackbar.CheckFail(response);
            }

            await Load_Click();
        }

        async Task Refuse_ClickAsync()
        {
            if (_selectedRequests == null || _selectedRequests.Count == 0)
                return;

            foreach (var request in _selectedRequests)
            {
                var response = await JoinRequestApiClient.HandleRequest(request.Id, new JoinRequestHandleCommandModel()
                {
                    IsAccepted = false
                });

                Snackbar.CheckFail(response);
            }

            await Load_Click();
        }
    }
}
