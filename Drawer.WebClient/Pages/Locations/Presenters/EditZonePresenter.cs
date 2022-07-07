using Drawer.WebClient.Api;
using Drawer.WebClient.Api.Locations;
using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.Locations.Presenters
{
    public class EditZonePresenter : SnackbarPresenter
    {
        private readonly ZoneApiClient _zoneApiClient;
        private readonly WorkPlaceApiClient _workPlaceApiClient;

        public IEditZoneView View { get; set; } = null!;

        public ZoneModel Model => View.Model;

        public EditZonePresenter(ZoneApiClient apiClient, ISnackbar snackbar, WorkPlaceApiClient workPlaceApiClient) : base(snackbar)
        {
            _zoneApiClient = apiClient;
            _workPlaceApiClient = workPlaceApiClient;
        }

        public async Task AddZoneAsync()
        {
            var response = await _zoneApiClient.AddZone(Model.WorkPlaceId, Model.Name, Model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful && response.Data != null)
                View.Model.Id = response.Data.Id;

            if (response.IsSuccessful)
                View.CloseView();
        }

        public async Task UpdateZoneAsync()
        {
            var response = await _zoneApiClient.UpdateZone(Model.Id, Model.Name, Model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
                View.CloseView();
        }

        public async Task LoadWorkPlaces()
        {
            var response = await _workPlaceApiClient.GetWorkPlaces();
            CheckFail(response);

            if (response.IsSuccessful && response.Data != null)
            {
                foreach(var workPlace in response.Data.WorkPlaces)
                {
                    var model = new WorkPlaceModel()
                    {
                        Id = workPlace.Id,
                        Name = workPlace.Name,
                        Note = workPlace.Note ?? string.Empty,
                    };
                    View.WorkPlaceModels.Add(model);
                }
            }

        }
    }
}
