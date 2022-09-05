using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Location.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Location
{
    public partial class LocationEdit
    {
        private MudForm? _form;
        private bool _isFormValid;
        private bool _isLoading;

        private readonly LocationModel _location = new();
        private readonly LocationModelValidator _validator = new();
        private readonly List<LocationGroupQueryModel> _locationGroups = new();

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
        [Inject] public LocationGroupApiClient GroupApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            var groupTask = GroupApiClient.GetLocationGroups();
            var locationTask = EditMode == EditMode.Update
                ? LocationApiClient.GetLocation(LocationId)
                : null;
            

            if (locationTask == null)
                await Task.WhenAll(groupTask);
            else
                await Task.WhenAll(locationTask, groupTask);

            var groupResponse = groupTask.Result;
            if (!Snackbar.CheckFail(groupResponse))
            {
                _isLoading = false;
                return;
            }

            _locationGroups.Clear();
            _locationGroups.AddRange(groupResponse.Data);
            _validator.GroupNames = _locationGroups.Select(x => x.Name).ToList();


            if (locationTask != null)
            {
                var locationResponse = locationTask.Result;
                var location = locationResponse.Data;
                if (location == null)
                {
                    Snackbar.Add("위치가 유효하지 않습니다", Severity.Error);
                    _isLoading = false;
                    return;
                }

                _location.Id = location.Id;
                _location.Name = location.Name;
                _location.Note = location.Note;
                _location.GroupId = location.GroupId;
                _location.GroupName = _locationGroups.First(x => x.Id == location.GroupId).Name;
            }

            _isLoading = false;
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.LocationHome);
        }

        async Task Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var locationDto = new LocationAddCommandModel()
                    {
                        GroupId = _location.GroupId,
                        Name = _location.Name!,
                        Note = _location.Note,
                    };
                    var response = await LocationApiClient.AddLocation(locationDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var locationDto = new LocationUpdateCommandModel()
                    {
                        Name = _location.Name!,
                        Note = _location.Note,
                    };
                    var response = await LocationApiClient.UpdateLocation(_location.Id, locationDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationHome);
                    }
                }
            }
        }

    }
}
