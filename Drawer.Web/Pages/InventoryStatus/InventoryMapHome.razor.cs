﻿using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services.Canvas;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PSC.Blazor.Components.BrowserDetect;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class InventoryMapHome : IAsyncDisposable
    {
        private const string CANVAS_ID = "canvas";

        /// <summary>
        /// 데이터 불러오기 작업. 작업이 완료된 후 초기 렌더링을 갱신한다.
        /// </summary>
        private Task? dataLoadTask;

        private readonly List<LayoutQueryModel> _layoutQueryModels = new();

        private readonly List<ItemQueryModel> _itemQueryModels = new();

        private readonly List<LocationQueryModel> _locationQueryModels = new();

        private readonly List<LocationGroupQueryModel> _locationGroupQueryModels = new();

        /// <summary>
        /// 재고아이템 쿼리모델. 아이템의 위치를 조회할 때 사용한다.
        /// </summary>
        private readonly List<InventoryItemQueryModel> _inventoryItemQueryModels = new();

        /// <summary>
        /// 수량합계 재고 아이템. 재고수량 테이블 데이터 소스.
        /// </summary>
        private readonly List<ItemQtyModel> _masterItemList = new();

        /// <summary>
        /// 재고아이템. 재고위치 테이블 데이터 소스.
        /// </summary>
        private readonly List<ItemQtyLocationModel> _detailItemList = new();

        /// <summary>
        /// 현재 선택된 루트 위치그룹
        /// </summary>
        private LocationGroupQueryModel? _selectedLocationGroup;

        /// <summary>
        /// 현재 선택된 아이템
        /// </summary>
        private ItemQtyModel? _focusedMasterItem;


        private AidTable<ItemQtyModel>? _masterItemTable;

        /// <summary>
        /// 데이터 로딩중 여부
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// 접근 권한
        /// </summary>
        private bool _canAccess = false;

        private string? _searchText;

        private bool _browerDetected;
        private bool _isMobile;
        private BrowserDetectJsInterop? browserDetectJsInterop;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationGroupApiClient LocationGroupApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public InventoryItemApiClient InventoryApiClient { get; set; } = null!;
        [Inject] public LayoutApiClient LayoutApiClient { get; set; } = null!;
        [Inject] public ICanvasService CanvasService { get; set; } = null!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

        public int MasterItemListCount => _masterItemList.Count;

        public int DetailItemListCount => _detailItemList.Count;

        public string? SearchText
        {
            get => _searchText;
            set
            {
                if (EqualityComparer<string>.Default.Equals(_searchText, value))
                    return;
                _searchText = value;
                var filteredItems = _masterItemTable?.GetFilteredItems();
                FocusedMasterItem = filteredItems?.FirstOrDefault();
            }
        }

        public LocationGroupQueryModel? SelectedLocationGroup
        {
            get => _selectedLocationGroup;
            set
            {
                if (_selectedLocationGroup?.Id == value?.Id)
                    return;
                _selectedLocationGroup = value;

                _masterItemList.Clear();
                _masterItemList.AddRange(_itemQueryModels.Select(item =>
                    new ItemQtyModel()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name,
                        // 루트 위치 조회
                        Quantity = _inventoryItemQueryModels.Where(x => 
                            x.ItemId == item.Id && GetRootLocationId(x.LocationId) == value?.Id)
                        .Sum(x => x.Quantity)
                    }));


                var layout = _layoutQueryModels.FirstOrDefault(x => x.LocationId == value?.Id);
                if (layout == null)
                {
                    CanvasService.ClearCanvas();
                    return;
                }

                CanvasService.ClearCanvas()
                    .ContinueWith((_) =>
                    {
                        CanvasService.ImportItemList(layout.ItemList.Select(x => CanvasItemConverter.ToCanvasItem(x)).ToList())
                            .ContinueWith((_) =>
                            {
                                CanvasService.SetInteraction(false);
                            });
                    });
            }
        }

        /// <summary>
        /// 선택된 재고 품목
        /// </summary>
        public ItemQtyModel? FocusedMasterItem
        {
            get => _focusedMasterItem;
            set
            {
                if (EqualityComparer<ItemQtyModel>.Default.Equals(_focusedMasterItem, value))
                    return;
                _focusedMasterItem = value;

                if (value == null)
                {
                    _detailItemList.Clear();
                    CanvasService.SetBlink(new List<string>());
                    return;
                }

                _detailItemList.Clear();
                _detailItemList.AddRange(_inventoryItemQueryModels
                    .Where(x => x.ItemId == value.ItemId)
                    .Where(x=> 0 < x.Quantity)
                    .Where(x=> GetRootLocationId(x.LocationId) == SelectedLocationGroup?.Id)
                    .Select(x => new ItemQtyLocationModel()
                    {
                        ItemId = x.ItemId,
                        ItemName = _itemQueryModels.FirstOrDefault(y => y.Id == x.ItemId)?.Name,
                        LocationId = x.LocationId,
                        LocationName = _locationQueryModels.FirstOrDefault(y => y.Id == x.LocationId)?.Name,
                        Quantity = x.Quantity
                    }));

                var selectedLayout = _layoutQueryModels.FirstOrDefault(x => x.LocationId == SelectedLocationGroup?.Id);
                if (selectedLayout == null)
                    return;

                var itemLocationList = _inventoryItemQueryModels
                    .Where(x => x.ItemId == value.ItemId && x.Quantity > 0)
                    .Select(x => x.LocationId);
                var layoutItemsToFlash = selectedLayout.ItemList
                    .Where(x => x.ConnectedLocations.Any(locId => itemLocationList.Contains(locId)));

                CanvasService.SetBlink(layoutItemsToFlash.Select(x => x.ItemId).ToList());
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _canAccess = true;
            dataLoadTask = Load_Click();
            await dataLoadTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                browserDetectJsInterop = new BrowserDetectJsInterop(JSRuntime);
                var info = await browserDetectJsInterop.BrowserInfo();

                _isMobile = info.IsMobile ?? false;
                _browerDetected = true;
                StateHasChanged();

                if (!_isMobile)
                {
                    await CanvasService.InitCanvas(CANVAS_ID, Enumerable.Empty<PaletteItem>(), new CanvasCallbacks(), false, true);
                    await CanvasService.Zoom(0.6);

                    await dataLoadTask!;
                    SelectedLocationGroup = _locationGroupQueryModels.FirstOrDefault();
                    StateHasChanged();
                }
            }
        }

        private bool FilterInventoryItem(ItemQtyModel masterItem)
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return true;
            if (masterItem == null)
                return false;

            return masterItem.ItemName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isLoading = true;

            var itemTask = ItemApiClient.GetItems();
            var groupTask = LocationGroupApiClient.GetLocationGroups();
            var locationTask = LocationApiClient.GetLocations();
            var inventoryTask = InventoryApiClient.GetInventoryDetails();
            var layoutTask = LayoutApiClient.GetLayouts();

            await Task.WhenAll(itemTask, groupTask, locationTask, inventoryTask, layoutTask);

            var itemResponse = itemTask.Result;
            var groupResponse = groupTask.Result;
            var locationResponse = locationTask.Result;
            var inventoryResponse = inventoryTask.Result;
            var layoutResponse = layoutTask.Result;

            if (!Snackbar.CheckFail(itemResponse, locationResponse, inventoryResponse, layoutResponse))
            {
                _isLoading = false;
                return;
            }

            _itemQueryModels.Clear();
            _locationQueryModels.Clear();
            _locationGroupQueryModels.Clear();
            _layoutQueryModels.Clear();
            _inventoryItemQueryModels.Clear();

            _itemQueryModels.AddRange(itemResponse.Data);
            _locationQueryModels.AddRange(locationResponse.Data);
            _locationGroupQueryModels.AddRange(groupResponse.Data);
            _layoutQueryModels.AddRange(layoutResponse.Data);
            _inventoryItemQueryModels.AddRange(inventoryResponse.Data);
 

            _isLoading = false;
        }

        private long GetRootLocationId(long locationId)
        {
            return _locationQueryModels.First(x => x.Id == locationId).RootGroupId;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (browserDetectJsInterop != null)
                await browserDetectJsInterop.DisposeAsync();

            await CanvasService.DisposeAsync();
        }
    }
}
