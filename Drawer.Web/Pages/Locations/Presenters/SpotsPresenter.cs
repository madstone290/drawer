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
    public class SpotsPresenter : SnackbarPresenter
    {
        private readonly IDialogService _dialogService;
        private readonly SpotApiClient _spotApiClient;
        private readonly ZoneApiClient _zoneApiClient;
        private readonly WorkPlaceApiClient _workPlaceApiClient;

        private readonly List<GetWorkPlacesResponse.WorkPlace> workPlaceList = new List<GetWorkPlacesResponse.WorkPlace>();
        private readonly List<GetZonesResponse.Zone> zoneList = new List<GetZonesResponse.Zone>();

        public SpotsPresenter(ISnackbar snackbar,
            IDialogService dialogService,
            SpotApiClient spotApiClient,
            ZoneApiClient zoneApiClient,
            WorkPlaceApiClient workPlaceApiClient) : base(snackbar)
        {
            _dialogService = dialogService;
            _spotApiClient = spotApiClient;
            _zoneApiClient = zoneApiClient;
            _workPlaceApiClient = workPlaceApiClient;
        }

        public ISpotsView View { get; set; } = null!;




        public async Task LoadSpotsAsync()
        {
            View.IsTableLoading = true;
            var spotReponse = await _spotApiClient.GetSpots();
            if (!CheckFail(spotReponse))
                return;

            var zoneResponse = await _zoneApiClient.GetZones();
            if (!CheckFail(zoneResponse))
                return;

            var workPlaceResponse = await _workPlaceApiClient.GetWorkPlaces();
            if (!CheckFail(workPlaceResponse))
                return;

            View.SpotList.Clear();
            zoneList.Clear();
            zoneList.AddRange(zoneResponse.Data.Zones);
            workPlaceList.Clear(); 
            workPlaceList.AddRange(workPlaceResponse.Data.WorkPlaces);

            foreach (var item in spotReponse.Data.Spots)
            {
                var zone = zoneList.First(x => x.Id == item.ZoneId);
                var workPlace = workPlaceList.First(x => x.Id == zone.WorkPlaceId); 
                var spot = new SpotTableModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note ?? string.Empty,
                    ZoneId = zone.Id,
                    ZoneName = zone.Name,
                    WorkPlaceId = workPlace.Id,
                    WorkPlaceName = workPlace.Name,
                };
                View.SpotList.Add(spot);
            }
            RefreshTotalRowCount();
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
                { nameof(EditSpotDialog.ActionMode), ActionMode.Add },
            };
            var dialog = _dialogService.Show<EditSpotDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var item = (SpotModel)result.Data;
                var zone = zoneList.First(x => x.Id == item.ZoneId);
                var workPlace = workPlaceList.First(x => x.Id == zone.WorkPlaceId);
                var zoneModel = new SpotTableModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note ?? string.Empty,
                    ZoneId = zone.Id,
                    ZoneName = zone.Name,
                    WorkPlaceId = workPlace.Id,
                    WorkPlaceName = workPlace.Name,
                };
                View.SpotList.Add(zoneModel);
                RefreshTotalRowCount();
            }
        }

        public async Task ShowUpdateDialog()
        {
            if (View.SelectedSpot == null)
            {
                _snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }
            var selectedItem = View.SelectedSpot;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditSpotDialog.ActionMode), ActionMode.Update },
                { nameof(EditSpotDialog.SpotModel), new SpotModel()
                    {
                        Id = selectedItem.Id,
                        ZoneId = selectedItem.ZoneId,
                        Name = selectedItem.Name,
                        Note = selectedItem.Note,
                    }
                }
            };
            var dialog = _dialogService.Show<EditSpotDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var workPlace = (SpotModel)result.Data;
                selectedItem.Name = workPlace.Name;
                selectedItem.Note = workPlace.Note;
            }
        }

        public async Task ShowDeleteDialog()
        {
            if (View.SelectedSpot == null)
            {
                _snackbar.Add("구역을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedItem = View.SelectedSpot;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"Id: {View.SelectedSpot.Id} 구역을 삭제하시겠습니까?" }
            };
            var dialog = _dialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _spotApiClient.DeleteSpot(selectedItem.Id);
                CheckSuccessFail(response);

                if (response.IsSuccessful)
                {
                    View.SpotList.Remove(selectedItem);
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
            View.TotalRowCount = View.SpotList.Count;
        }
    }
}


