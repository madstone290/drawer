using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Locations
{
    public partial class ZoneEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly ZoneModel _zone = new();
        private readonly ZoneModelValidator _validator = new();
        private readonly List<WorkplaceModel> _workplaceList = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "구역 추가";
                else if (EditMode == EditMode.Update)
                    return "구역 수정";
                else
                    return "구역 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long? ZoneId { get; set; }

        [Inject] public ZoneApiClient ZoneApiClient { get; set; } = null!;
        [Inject] public WorkplaceApiClient WorkplaceApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (ZoneId.HasValue)
            {
                var response = await ZoneApiClient.GetZone(ZoneId.Value);
                if (Snackbar.CheckFail(response))
                {
                    _zone.Id = response.Data.Id;
                    _zone.Name = response.Data.Name;
                    _zone.Note = response.Data.Note;
                    _zone.WorkplaceId = response.Data.WorkplaceId;
                }
            }

            var workplaceResponse = await WorkplaceApiClient.GetWorkplaces();
            if (Snackbar.CheckFail(workplaceResponse))
            {
                foreach (var workPlace in workplaceResponse.Data.Workplaces)
                {
                    _workplaceList.Add(new WorkplaceModel()
                    {
                        Id = workPlace.Id,
                        Name = workPlace.Name,
                        Note = workPlace.Note
                    });
                }
            }

        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.ZoneHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateZoneRequest(_zone.WorkplaceId, _zone.Name!, _zone.Note);
                    var response = await ZoneApiClient.AddZone(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ZoneHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateZoneRequest(_zone.Name!, _zone.Note);
                    var response = await ZoneApiClient.UpdateZone(_zone.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.ZoneHome);
                    }
                }

            }
        }

        Task<IEnumerable<long>> FilterWorkPlaceIds(string filterText)
        {
            var filterResult = filterText == null
                ? _workplaceList
                : _workplaceList.Where(x => x.Name != null && x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(filterResult.Select(x => x.Id));
        }

        string? DisplayWorkPlaceName(long id)
        {
            return _workplaceList.FirstOrDefault(x => x.Id == id)?.Name;
        }
    }
}
