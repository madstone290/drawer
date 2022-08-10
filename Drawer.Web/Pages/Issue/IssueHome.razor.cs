using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Issue.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Issue
{
    public partial class IssueHome
    {
        private AidTable<IssueTableModel> table = null!;
        private readonly List<IssueTableModel> _issueList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(IssueTableModel.TransactionNumber), "출고번호")
            .AddColumn(nameof(IssueTableModel.IssueDateString), "출고일자")
            .AddColumn(nameof(IssueTableModel.IssueTimeString), "출고시간")
            .AddColumn(nameof(IssueTableModel.ItemName), "아이템")
            .AddColumn(nameof(IssueTableModel.LocationName), "위치")
            .AddColumn(nameof(IssueTableModel.QuantityString), "수량")
            .AddColumn(nameof(IssueTableModel.Buyer), "구매자")
            .Build();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        /// <summary>
        /// 조회 시작일
        /// </summary>
        private DateTime? _issueDateFrom = DateTime.Today;
        /// <summary>
        /// 조회 종료일
        /// </summary>
        private DateTime? _issueDateTo = DateTime.Today;

        [Inject] public IssueApiClient IssueApiClient { get; set; } = null!;
        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public IssueTableModel? SelectedIssue => table.FocusedItem;
        public int TotalRowCount => _issueList.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterIssues(IssueTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.TransactionNumber?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.IssueDateString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.IssueTimeString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.ItemName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.LocationName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.QuantityString?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.Buyer?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            if (!_issueDateFrom.HasValue || !_issueDateTo.HasValue)
            {
                Snackbar.Add("조회 기간을 선택하세요");
                return;
            }

            _isTableLoading = true;
            var issueResponse = await IssueApiClient.GetIssues(_issueDateFrom.Value, _issueDateTo.Value);
            var itemResponse = await ItemApiClient.GetItems();
            var locationResponse = await LocationApiClient.GetLocations();
            if (!Snackbar.CheckFail(issueResponse, itemResponse, locationResponse))
            {
                _isTableLoading = false;
                return;
            }

            var itemList = itemResponse.Data;
            var locationList = locationResponse.Data;
            _issueList.Clear();
            foreach (var issueDto in issueResponse.Data)
            {
                var issue = new IssueTableModel()
                {
                    Id = issueDto.Id,
                    TransactionNumber = issueDto.TransactionNumber,
                    IssueDateString = issueDto.IssueDateTimeLocal.Date.ToString("yyyy-MM-dd"),
                    IssueTimeString = issueDto.IssueDateTimeLocal.TimeOfDay.ToString(@"hh\:mm"),
                    ItemId = issueDto.ItemId,
                    ItemName = itemList.First(x => x.Id == issueDto.ItemId).Name,
                    LocationId = issueDto.LocationId,
                    LocationName = locationList.First(x => x.Id == issueDto.LocationId).Name,
                    QuantityString = issueDto.Quantity.ToString(),
                    Buyer = issueDto.Buyer
                };
                _issueList.Add(issue);
            }

            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.IssueAdd);
        }

        private void Update_Click()
        {

            if (SelectedIssue == null)
            {
                Snackbar.Add("출고를 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.IssueUpdate.Replace("{id}", $"{SelectedIssue.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedIssue == null)
            {
                Snackbar.Add("출고 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedIssue = SelectedIssue;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedIssue.TransactionNumber} 출고를 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await IssueApiClient.DeleteIssue(selectedIssue.Id);
                if (Snackbar.CheckSuccessFail(response))
                {
                    _issueList.Remove(selectedIssue);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.IssueBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"출고-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _issueList, _excelOptions);
        }
    }
}
