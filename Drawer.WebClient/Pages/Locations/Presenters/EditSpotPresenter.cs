using Drawer.WebClient.Api.Locations;
using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.Locations.Presenters
{
    public class EditSpotPresenter : SnackbarPresenter
    {
        private readonly SpotApiClient _spotApiClient;
        private readonly ZoneApiClient _zoneApiClient;

        public IEditSpotView View { get; set; } = null!;

        public SpotModel Model => View.SpotModel;

        public EditSpotPresenter(ISnackbar snackbar, SpotApiClient apiClient, ZoneApiClient zoneApiClient) : base(snackbar)
        {
            _spotApiClient = apiClient;
            _zoneApiClient = zoneApiClient;
        }

        public async Task AddSpotAsync()
        {
            var response = await _spotApiClient.AddSpot(Model.ZoneId, Model.Name, Model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful && response.Data != null)
                View.SpotModel.Id = response.Data.Id;

            if (response.IsSuccessful)
                View.CloseView();
        }

        public async Task UpdateSpotAsync()
        {
            var response = await _spotApiClient.UpdateSpot(Model.Id, Model.Name, Model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
                View.CloseView();
        }

        public async Task LoadZones()
        {
            var response = await _zoneApiClient.GetZones();
            CheckFail(response);

            if (response.IsSuccessful && response.Data != null)
            {
                foreach(var item in response.Data.Zones)
                {
                    var zone = new ZoneModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Note = item.Note ?? string.Empty,
                    };
                    View.ZoneModels.Add(zone);
                }
            }

        }
    }
}
