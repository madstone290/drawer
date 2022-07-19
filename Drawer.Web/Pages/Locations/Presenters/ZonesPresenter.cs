using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Components;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Views;
using Drawer.Web.Presenters;
using Drawer.Web.Shared;
using MudBlazor;

namespace Drawer.Web.Pages.Locations.Presenters
{
    public class ZonesPresenter : SnackbarPresenter
    {
        private readonly ZoneApiClient _zoneApiClient;
        private readonly WorkPlaceApiClient _workPlaceApiClient;

        private readonly IDialogService _dialogService;

        private List<GetWorkPlacesResponse.WorkPlace> workPlaceList = new List<GetWorkPlacesResponse.WorkPlace>();

        public IZonesView View { get; set; } = null!;

        public ZonesPresenter(ZoneApiClient apiClient, IDialogService dialogService, ISnackbar snackbar, WorkPlaceApiClient workPlaceApiClient) : base(snackbar)
        {
            _zoneApiClient = apiClient;
            _dialogService = dialogService;
            _workPlaceApiClient = workPlaceApiClient;
        }

        public async Task LoadZonesAsync()
        {
            View.IsTableLoading = true;
            var zoneResponse = await _zoneApiClient.GetZones();
            CheckFail(zoneResponse);

            var workPlaceResponse = await _workPlaceApiClient.GetWorkPlaces();
            CheckFail(workPlaceResponse);

            if (zoneResponse.IsSuccessful && zoneResponse.Data != null &&
                workPlaceResponse.IsSuccessful && workPlaceResponse.Data != null)
            {
                View.ZoneList.Clear();
                workPlaceList.Clear();
                workPlaceList.AddRange(workPlaceResponse.Data.WorkPlaces);
                
                foreach (var item in zoneResponse.Data.Zones)
                {
                    var workPlaceModel = new ZoneTableModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Note = item.Note ?? string.Empty,
                        WorkPlaceId = item.WorkPlaceId,
                        WorkPlaceName = workPlaceList.FirstOrDefault(x=> x.Id == item.WorkPlaceId)?.Name ?? string.Empty,
                    };
                    View.ZoneList.Add(workPlaceModel);
                }
                RefreshTotalRowCount();
            }
            View.IsTableLoading = false;
        }

        public async Task ShowAddDialog()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditZoneDialog.ActionMode), ActionMode.Add },
            };
            var dialog = _dialogService.Show<EditZoneDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var item = (ZoneModel)result.Data;
                var zoneModel = new ZoneTableModel()
                {
                    Id = item.Id,
                    WorkPlaceId = item.Id,
                    Name = item.Name,
                    Note = item.Note ?? string.Empty,
                    WorkPlaceName = workPlaceList.FirstOrDefault(x => x.Id == item.WorkPlaceId)?.Name ?? string.Empty,
                };
                View.ZoneList.Add(zoneModel);
                RefreshTotalRowCount();
            }
        }

        public async Task ShowUpdateDialog()
        {
            if (View.SelectedZone == null)
            {
                _snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }
            var selectedItem = View.SelectedZone;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditZoneDialog.ActionMode), ActionMode.Update },
                { nameof(EditZoneDialog.Model), new ZoneModel()
                    {
                        Id = selectedItem.Id,
                        WorkPlaceId = selectedItem.WorkPlaceId,
                        Name = selectedItem.Name,
                        Note = selectedItem.Note,
                    }
                }
            };
            var dialog = _dialogService.Show<EditZoneDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var workPlace = (ZoneModel)result.Data;
                selectedItem.Name = workPlace.Name;
                selectedItem.Note = workPlace.Note;
            }
        }

        public async Task ShowDeleteDialog()
        {
            if (View.SelectedZone == null)
            {
                _snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedItem = View.SelectedZone;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"Id: {View.SelectedZone.Id} 구역을 삭제하시겠습니까?" }
            };
            var dialog = _dialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _zoneApiClient.DeleteZone(selectedItem.Id);
                CheckSuccessFail(response);

                if (response.IsSuccessful)
                {
                    View.ZoneList.Remove(selectedItem);
                    RefreshTotalRowCount();
                }
            }
        }

        public void UploadExcel()
        {
            _snackbar.Add("구현 예정입니다");
        }

        public void DownloadExcel()
        {
            _snackbar.Add("구현 예정입니다");
        }

        private void RefreshTotalRowCount()
        {
            View.TotalRowCount = View.ZoneList.Count;
        }
    }
}


