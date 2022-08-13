﻿using Drawer.Application.Services.Inventory.CommandModels;
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
        private MudForm _form = null!;
        private bool _isFormValid;
        private bool _isLoading;

        private readonly LocationModel _location = new();
        private readonly LocationModelValidator _validator = new();
        private readonly List<LocationQueryModel> _locations = new();
        private readonly List<LocationQueryModel> _locationGroups = new();

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
            _isLoading = true;

            var response = await LocationApiClient.GetLocations();
            if (Snackbar.CheckFail(response))
            {
                _locations.Clear();
                _locations.AddRange(response.Data);

                _locationGroups.Clear();
                _locationGroups.AddRange(_locations.Where(x => x.IsGroup));

                _validator.LocationNames = _locationGroups.Select(x => x.Name).ToList();
            }


            if (EditMode == EditMode.Update)
            {
                var location = _locations.FirstOrDefault(x => x.Id == LocationId);
                if (location == null)
                    return;
                _location.Id = location.Id;
                _location.Name = location.Name;
                _location.Note = location.Note;
                _location.ParentGroupId = location.ParentGroupId ?? 0;
            }

            _isLoading = false;
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
                    var locationDto = new LocationAddCommandModel()
                    {
                        ParentGroupId = _location.ParentGroupId,
                        Name = _location.Name!,
                        Note = _location.Note,
                        IsGroup = _location.IsGroup
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