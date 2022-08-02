using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Locations
{
    public partial class SpotEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly SpotModel _spot = new();
        private readonly SpotModelValidator _validator = new();
        private readonly List<ZoneModel> _zoneList = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "자리 추가";
                else if (EditMode == EditMode.Update)
                    return "자리 수정";
                else
                    return "자리 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long? SpotId { get; set; }

        [Inject] public SpotApiClient SpotApiClient { get; set; } = null!;
        [Inject] public ZoneApiClient ZoneApiClient { get; set; } = null!;
        

        protected override async Task OnInitializedAsync()
        {
            if (SpotId.HasValue)
            {
                var response = await SpotApiClient.GetSpot(SpotId.Value);
                if (Snackbar.CheckFail(response))
                {
                    _spot.Id = response.Data.Id;
                    _spot.Name = response.Data.Name;
                    _spot.Note = response.Data.Note;
                    _spot.ZoneId = response.Data.ZoneId;
                }
            }

            var zoneResponse = await ZoneApiClient.GetZones();
            if (Snackbar.CheckFail(zoneResponse))
            {
                foreach (var zone in zoneResponse.Data.Zones)
                {
                    _zoneList.Add(new ZoneModel()
                    {
                        Id = zone.Id,
                        Name = zone.Name,
                        Note = zone.Note,
                        WorkplaceId = zone.WorkplaceId
                    });
                }
            }

        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.SpotHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateSpotRequest(_spot.ZoneId, _spot.Name!, _spot.Note);
                    var response = await SpotApiClient.AddSpot(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.SpotHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateSpotRequest(_spot.Name!, _spot.Note);
                    var response = await SpotApiClient.UpdateSpot(_spot.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.SpotHome);
                    }
                }

            }
        }
        Task<IEnumerable<long>> FilterZoneIds(string filterText)
        {
            var filterResult = filterText == null
                ? _zoneList
                : _zoneList.Where(x => x.Name != null && x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(filterResult.Select(x => x.Id));
        }

        string? DisplayZoneName(long id)
        {
            return _zoneList.FirstOrDefault(x => x.Id == id)?.Name;
        }

    }
}
