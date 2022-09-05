using Drawer.AidBlazor;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Layout.Models;
using Drawer.Web.Services.Canvas;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Layout
{
    public partial class LayoutHome : IAsyncDisposable
    {
        private const string CANVAS_ID = "canvas";

        private readonly List<LocationGroupModel> _rootGroupList = new();
        private readonly List<LayoutModel> _layoutList = new();

        private LocationGroupModel? selectedGroup;

        private bool _canAccess = true;
        private bool _isLoading = false;

        public int TotalRowCount => _rootGroupList.Count;

        [Inject] public LocationGroupApiClient LocationGroupApiClient { get; set; } = null!;
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
                await CanvasService.InitCanvas(CANVAS_ID, Enumerable.Empty<PaletteItem>(), new CanvasCallbacks(), false);
            }
        }

        async void OnFocusedItemChanged(LocationGroupModel location)
        {
            selectedGroup = location;
            
            await CanvasService.ClearCanvas();

            if (location == null)
                return;

            await CanvasService.Zoom(0.6);

            var layout = _layoutList.First(x => x.LocationGroupId == location.Id);
            await CanvasService.ImportItemList(
                layout.ItemList.Select(x => CanvasItemConverter.ToCanvasItem(x)).ToList());

            await CanvasService.SetInteraction(false);
  
        }

        async Task Load_Click()
        {
            _isLoading = true;
            var groupTask = LocationGroupApiClient.GetLocationGroups();
            var layoutTask = LayoutApiClient.GetLayouts();
            await Task.WhenAll(groupTask, layoutTask);

            var groupResponse = groupTask.Result;
            var layoutResponse = layoutTask.Result;

            if (!Snackbar.CheckFail(groupResponse) || !Snackbar.CheckFail(layoutResponse))
            {
                _isLoading = false;
                return;
            }

            _rootGroupList.Clear();
            foreach (var groupDto in groupResponse.Data.Where(x=> x.IsRoot))
            {
                var group = new LocationGroupModel()
                {
                    Id = groupDto.Id,
                    Name = groupDto.Name,
                    Note = groupDto.Note,
                };
                _rootGroupList.Add(group);
            }

            _layoutList.Clear();
            foreach (var location in _rootGroupList)
            {
                var layoutDto = layoutResponse.Data.FirstOrDefault(x => x.LocationId == location.Id);
                var layout = new LayoutModel()
                {
                    LocationGroupId = location.Id,
                    ItemList = layoutDto?.ItemList ?? new List<Domain.Models.Inventory.LayoutItem>()
                };

                _layoutList.Add(layout);
            }


            _isLoading = false;
        }

        void Edit_Click()
        {
            if (selectedGroup != null)
            {
                NavManager.NavigateTo(Paths.LayoutEdit.AddQuery("LocationGroupId", $"{selectedGroup.Id}"));
            }
        }

        void Delete_Click()
        {

        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await CanvasService.DisposeAsync();
        }
    }
}


