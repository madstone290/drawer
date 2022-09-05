using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
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
        private readonly List<ItemQueryModel> _itemList = new();
        private readonly List<LocationQueryModel> _locationList = new();

        private bool _isLoading;

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
            _isLoading = true;

            var itemTask = ItemApiClient.GetItems()
                .ContinueWith((task) =>
                {
                    var itemResponse = task.Result;
                    if (!Snackbar.CheckFail(itemResponse))
                        return;

                    _itemList.Clear();
                    _itemList.AddRange(itemResponse.Data);
                    _validator.ItemNames = _itemList.Select(x => x.Name).ToList();
                });

            var locationTask = LocationApiClient.GetLocations()
                .ContinueWith((task) =>
                {
                    var locationResponse = task.Result;
                    if (!Snackbar.CheckFail(locationResponse))
                        return;

                    _locationList.Clear();
                    _locationList.AddRange(locationResponse.Data);
                    _validator.LocationNames = _locationList.Select(x => x.Name).ToList();
                });

            var issueTask = EditMode != EditMode.Update
                ? Task.CompletedTask
                : IssueApiClient.GetIssue(IssueId)
                    .ContinueWith((task) =>
                    {
                        var issueResponse = task.Result;
                        if (!Snackbar.CheckFail(issueResponse))
                            return;

                        var issueDto = issueResponse.Data;
                        if (issueDto == null)
                        {
                            Snackbar.Add("출고내역을 조회할 수 없습니다", Severity.Error);
                            return;
                        }

                        _issue.Id = issueDto.Id;
                        _issue.IssueDate = issueDto.IssueDateTimeLocal.Date;
                        _issue.IssueTime = issueDto.IssueDateTimeLocal.TimeOfDay;
                        _issue.ItemId = issueDto.ItemId;
                        _issue.ItemName = _itemList.First(x => x.Id == issueDto.ItemId).Name;
                        _issue.LocationId = issueDto.LocationId;
                        _issue.LocationName = _locationList.First(x => x.Id == issueDto.LocationId).Name;
                        _issue.Quantity = issueDto.Quantity;
                        _issue.Buyer = issueDto.Buyer;
                    });

            await Task.WhenAll(itemTask, locationTask, issueTask);

            _isLoading = false;
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
                var issueDto = new IssueCommandModel()
                {
                    IssueDateTimeLocal = _issue.IssueDateTime,
                    ItemId = _issue.ItemId,
                    LocationId = _issue.LocationId,
                    Quantity = _issue.Quantity,
                    Buyer = _issue.Buyer
                };

                if (EditMode == EditMode.Add)
                {
                    var response = await IssueApiClient.AddIssue(issueDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.IssueHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var response = await IssueApiClient.UpdateIssue(_issue.Id, issueDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.IssueHome);
                    }
                }
            }
        }

    }
}

