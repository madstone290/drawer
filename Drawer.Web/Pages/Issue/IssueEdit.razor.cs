using Drawer.Contract.Inventory;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Issue.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Issue
{
    public partial class IssueEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly IssueModel _issue = new();
        private readonly IssueModelValidator _validator = new();
        private readonly List<GetItemsResponse.Item> _itemList = new();
        private readonly List<GetLocationsResponse.Location> _locationList = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "출고 추가";
                else if (EditMode == EditMode.Update)
                    return "출고 수정";
                else
                    return "출고 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long IssueId { get; set; }

        [Inject] public IssueApiClient IssueApiClient { get; set; } = null!;
        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {

            var itemResponse = await ItemApiClient.GetItems();
            var locationResponse = await LocationApiClient.GetLocations();
            if (!Snackbar.CheckFail(itemResponse, locationResponse))
                return;

            _itemList.Clear();
            _itemList.AddRange(itemResponse.Data.Items);

            _locationList.Clear();
            _locationList.AddRange(locationResponse.Data.Locations.Where(x => x.IsGroup == false));

            _validator.ItemNames = _itemList.Select(x => x.Name).ToList();
            _validator.LocationNames = _locationList.Select(x => x.Name).ToList();

            if (EditMode == EditMode.Update)
            {
                var issueResponse = await IssueApiClient.GetIssue(IssueId);
                if (!Snackbar.CheckFail(issueResponse))
                    return;

                var issueDto = issueResponse.Data.Issue;

                _issue.Id = issueDto.Id;
                _issue.IssueDate = issueDto.IssueDateTime.Date;
                _issue.IssueTime = issueDto.IssueDateTime.TimeOfDay;
                _issue.ItemId = issueDto.ItemId;
                _issue.ItemName = _itemList.First(x => x.Id == issueDto.ItemId).Name;
                _issue.LocationId = issueDto.LocationId;
                _issue.LocationName = _locationList.First(x => x.Id == issueDto.LocationId).Name;
                _issue.Quantity = issueDto.Quantity;
                _issue.Buyer = issueDto.Buyer;
            }


        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.IssueHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateIssueRequest(_issue.IssueDateTime, _issue.ItemId, _issue.LocationId,
                        _issue.Quantity, _issue.Buyer);
                    var response = await IssueApiClient.AddIssue(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.IssueHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateIssueRequest(_issue.IssueDateTime, _issue.ItemId, _issue.LocationId,
                        _issue.Quantity, _issue.Buyer);
                    var response = await IssueApiClient.UpdateIssue(_issue.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.IssueHome);
                    }
                }
            }
        }

    }
}

