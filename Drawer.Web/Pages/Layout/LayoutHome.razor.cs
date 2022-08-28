﻿using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Layout.Models;
using Drawer.Web.Services.Canvas;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Layout
{
    public partial class LayoutHome
    {
        private const string CANVAS_ID = "canvas";

        private readonly List<LocationModel> _locationList = new List<LocationModel>();
        private readonly List<LayoutModel> _layoutList = new List<LayoutModel>();

        private LocationModel? selectedLocation;

        private bool _canAccess = true;
        private bool _isLoading = false;
        private int _canvasWidth = 1000;
        private int _canvasHeight = 700;

        public int TotalRowCount => _locationList.Count;

        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public LayoutApiClient LayoutApiClient { get; set; } = null!;
        [Inject] public ICanvasService CanvasService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Load_Click();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CanvasService.InitCanvas(CANVAS_ID, Enumerable.Empty<PaletteItem>(), new CanvasCallbacks());
            }
        }

        async void OnFocusedItemChanged(LocationModel location)
        {
            selectedLocation = location;
            
            await CanvasService.ClearCanvas();

            if (location == null)
                return;

            var layout = _layoutList.First(x => x.LocationId == location.Id);
            await CanvasService.ImportItemList(
                layout.ItemList.Select(x => CanvasItemConverter.ToCanvasItem(x)).ToList());
        }

        async Task Load_Click()
        {
            _isLoading = true;
            var locationTask = LocationApiClient.GetLocations();
            var layoutTask = LayoutApiClient.GetLayouts();
            await Task.WhenAll(locationTask, layoutTask);

            var locationResponse = locationTask.Result;
            var layoutResponse = layoutTask.Result;

            if (!Snackbar.CheckFail(locationResponse) || !Snackbar.CheckFail(layoutResponse))
            {
                _isLoading = false;
                return;
            }

            _locationList.Clear();
            foreach (var locationDto in locationResponse.Data.Where(x => x.IsGroup && x.HierarchyLevel == 0))
            {
                var location = new LocationModel()
                {
                    Id = locationDto.Id,
                    Name = locationDto.Name,
                    Note = locationDto.Note,
                };
                _locationList.Add(location);
            }

            _layoutList.Clear();
            foreach (var location in _locationList)
            {
                var layoutDto = layoutResponse.Data.FirstOrDefault(x => x.LocationId == location.Id);
                var layout = new LayoutModel()
                {
                    LocationId = location.Id,
                    ItemList = layoutDto?.ItemList ?? new List<Domain.Models.Inventory.LayoutItem>()
                };

                _layoutList.Add(layout);
            }


            _isLoading = false;
        }

        void Edit_Click()
        {
            if (selectedLocation != null)
            {
                NavManager.NavigateTo(Paths.LayoutEdit.AddQuery("locationId", $"{selectedLocation.Id}"));
            }
        }

        void Delete_Click()
        {

        }
    }
}

