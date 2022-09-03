using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.LocationGroup.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.LocationGroup
{
    public partial class LocationGroupHome
    {
        private AidTable<LocationGroupModel>? table;
        private readonly List<LocationGroupModel> _locations = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(LocationGroupModel.ParentGroupName), "상위 그룹")
            .AddColumn(nameof(LocationGroupModel.Name), "이름")
            .AddColumn(nameof(LocationGroupModel.Note), "비고")
            .AddColumn(nameof(LocationGroupModel.Depth), "깊이")
            .Build();

        private bool _isTableLoading;
        private bool _canAccess = true;

        private string searchText = string.Empty;

        [Inject] public LocationGroupApiClient LocationGroupApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public LocationGroupModel? SelectedLocationGroup => table?.FocusedItem;
        public int TotalRowCount => _locations.Count;
        

        protected override async Task OnInitializedAsync()
        {
            await Load_Click();
        }

        private bool FilterLocationGroups(LocationGroupModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true|| 
                model.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true||
                model.ParentGroupName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;
            var response = await LocationGroupApiClient.GetLocationGroups();
            if (!Snackbar.CheckFail(response))
            {
                _isTableLoading = false;
                return;
            }

            _locations.Clear();
            foreach (var groupDto in response.Data)
            {
                var group = new LocationGroupModel()
                {
                    Id = groupDto.Id,
                    Name = groupDto.Name,
                    Note = groupDto.Note,
                    ParentGroupId = groupDto.ParentGroupId ?? 0,
                    ParentGroupName = response.Data.FirstOrDefault(x => x.Id == groupDto.ParentGroupId)?.Name,
                    Depth = groupDto.Depth,
                    RootGroupId = groupDto.RootGroupId
                };
                _locations.Add(group);
            }

            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.LocationGroupAdd);
        }

        private void Update_Click()
        {

            if (SelectedLocationGroup == null)
            {
                Snackbar.Add("위치를 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.LocationGroupUpdate.Replace("{id}", $"{SelectedLocationGroup.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedLocationGroup == null)
            {
                Snackbar.Add("위치를 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedLocationGroup = SelectedLocationGroup;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedLocationGroup.Name} 위치를 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await LocationGroupApiClient.DeleteLocationGroup(selectedLocationGroup.Id);
                if(Snackbar.CheckSuccessFail(response))
                {
                    _locations.Remove(selectedLocationGroup);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.LocationGroupBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"위치-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _locations, _excelOptions);
        }
    }
}
