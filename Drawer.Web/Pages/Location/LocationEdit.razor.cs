using Drawer.Contract.InventoryManagement;
using Drawer.Contract.Locations;
using Drawer.Web.Api.InventoryManagement;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Location.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Location
{
    public partial class LocationEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly LocationModel _location = new();
        private readonly LocationModelValidator _validator = new();
        private readonly List<GetLocationsResponse.Location> _locations = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "위치 추가";
                else if (EditMode == EditMode.Update)
                    return "위치 수정";
                else
                    return "위치 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long LocationId { get; set; }

        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            var response = await LocationApiClient.GetLocations();
            if (Snackbar.CheckFail(response))
            {
                _locations.Clear();
                _locations.AddRange(response.Data.Locations);
            }


            if (EditMode == EditMode.Update)
            {
                var location = _locations.FirstOrDefault(x => x.Id == LocationId);
                if (location == null)
                    return;
                _location.Id = location.Id;
                _location.Name = location.Name;
                _location.Note = location.Note;
                _location.UpperLocationId = location.UpperLocationId ?? 0;
            }
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.LocationHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateLocationRequest(_location.UpperLocationId, _location.Name!, _location.Note);
                    var response = await LocationApiClient.AddLocation(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateLocationRequest(_location.Name!, _location.Note);
                    var response = await LocationApiClient.UpdateLocation(_location.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationHome);
                    }
                }

            }
        }

        private string? ValidateUpperLocation(LocationModel location)
        {
            if (string.IsNullOrWhiteSpace(location.UpperLocationName))
                return null;
            if (_locations.Any(l => string.Equals(l.Name, location.UpperLocationName, StringComparison.OrdinalIgnoreCase)))
                return null;
            else
                return "잘못된 위치입니다";
        }

        
      
    }
}
