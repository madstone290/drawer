using Drawer.AidBlazor;
using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.LocationOld.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Drawer.Web.Pages.LocationOld
{
    public partial class ZoneHome
    {
        private AidTable<ZoneTableModel> table = null!;
        private readonly List<GetWorkplacesResponse.Workplace> _workPlaceList = new();
        private readonly List<ZoneTableModel> _zoneList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(ZoneTableModel.Name), "이름")
            .AddColumn(nameof(ZoneTableModel.Note), "비고")
            .AddColumn(nameof(ZoneTableModel.WorkplaceName), "작업장")
            .Build();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public ZoneApiClient ZoneApiClient { get; set; } = null!;
        [Inject] public WorkplaceApiClient WorkPlaceApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public ZoneTableModel? SelectedZone => table.FocusedItem;
        public int TotalRowCount => _zoneList.Count;
        

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterZones(ZoneTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true|| 
                model.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true|| 
                model.WorkplaceName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;
            var zoneResponse = await ZoneApiClient.GetZones();
            if (!Snackbar.CheckFail(zoneResponse))
            {
                _isTableLoading = false;
                return;
            }

            var workPlaceResponse = await WorkPlaceApiClient.GetWorkplaces();
            if (!Snackbar.CheckFail(workPlaceResponse))
            {
                _isTableLoading = false;
                return;
            }

            if (zoneResponse.IsSuccessful && zoneResponse.Data != null &&
                workPlaceResponse.IsSuccessful && workPlaceResponse.Data != null)
            {
                _zoneList.Clear();
                _workPlaceList.Clear();
                _workPlaceList.AddRange(workPlaceResponse.Data.Workplaces);

                foreach (var item in zoneResponse.Data.Zones)
                {
                    var workPlaceModel = new ZoneTableModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Note = item.Note,
                        WorkplaceId = item.WorkplaceId,
                        WorkplaceName = _workPlaceList.FirstOrDefault(x => x.Id == item.WorkplaceId)?.Name,
                    };
                    _zoneList.Add(workPlaceModel);
                }
            }
            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.ZoneAdd);
        }

        private void Update_Click()
        {

            if (SelectedZone == null)
            {
                Snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.ZoneUpdate.Replace("{id}", $"{SelectedZone.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedZone == null)
            {
                Snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedZone = SelectedZone;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedZone.Name} 구역을 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ZoneApiClient.DeleteZone(selectedZone.Id);
                if(Snackbar.CheckSuccessFail(response))
                {
                    _zoneList.Remove(selectedZone);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.ZoneBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"작업장-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _zoneList, _excelOptions);
        }
    }
}
