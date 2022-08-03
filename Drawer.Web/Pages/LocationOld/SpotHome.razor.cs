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
    public partial class SpotHome
    {
        private AidTable<SpotTableModel> _table = null!;
        private bool _isTableLoading;
        private List<SpotTableModel> _spotList = new();
        private readonly List<GetWorkplacesResponse.Workplace> _workPlaceList = new();
        private readonly List<GetZonesResponse.Zone> _zoneList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(SpotTableModel.Name), "이름")
            .AddColumn(nameof(SpotTableModel.Note), "비고")
            .AddColumn(nameof(SpotTableModel.WorkplaceName), "작업장")
            .AddColumn(nameof(SpotTableModel.ZoneName), "구역")
            .Build();

        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;



        [Inject] public SpotApiClient SpotApiClient { get; set; } = null!;
        [Inject] public ZoneApiClient ZoneApiClient { get; set; } = null!;
        [Inject] public WorkplaceApiClient WorkPlaceApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public SpotTableModel? SelectedSpot => _table.FocusedItem;

        public int TotalRowCount => _spotList.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterSpots(SpotTableModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                model.WorkplaceName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true||
                model.ZoneName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;
            var spotReponse = await SpotApiClient.GetSpots();
            if (!Snackbar.CheckFail(spotReponse))
            {
                _isTableLoading = false;
                return;
            }

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

            _spotList.Clear();
            _zoneList.Clear();
            _zoneList.AddRange(zoneResponse.Data.Zones);
            _workPlaceList.Clear();
            _workPlaceList.AddRange(workPlaceResponse.Data.Workplaces);

            foreach (var item in spotReponse.Data.Spots)
            {
                var zone = _zoneList.First(x => x.Id == item.ZoneId);
                var workPlace = _workPlaceList.First(x => x.Id == zone.WorkplaceId);
                var spot = new SpotTableModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note,
                    ZoneId = zone.Id,
                    ZoneName = zone.Name,
                    WorkplaceId = workPlace.Id,
                    WorkplaceName = workPlace.Name,
                };
                _spotList.Add(spot);
            }
            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.SpotAdd);
        }

        private void Update_Click()
        {
            if (SelectedSpot == null)
            {
                Snackbar.Add("자리를 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.SpotUpdate.Replace("{id}", $"{SelectedSpot.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedSpot == null)
            {
                Snackbar.Add("자리를 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedSpot = SelectedSpot;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedSpot.Name} 자리를 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await SpotApiClient.DeleteSpot(selectedSpot.Id);
                if (Snackbar.CheckSuccessFail(response))
                {
                    _spotList.Remove(selectedSpot);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.SpotBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"자리-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _spotList, _excelOptions);
        }
    }
}
