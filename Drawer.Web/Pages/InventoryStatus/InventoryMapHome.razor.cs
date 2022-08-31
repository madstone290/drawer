﻿using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services.Canvas;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class InventoryMapHome
    {
        private const string CANVAS_ID = "canvas";

        private readonly List<LayoutQueryModel> _layoutQueryModels = new();

        private readonly List<ItemQueryModel> _itemQueryModels = new();

        private readonly List<LocationQueryModel> _locationQueryModels = new();

        private readonly List<LocationQueryModel> _rootLocationGroups = new();

        /// <summary>
        /// 재고아이템 쿼리모델. 아이템의 위치를 조회할 때 사용한다.
        /// </summary>
        private readonly List<InventoryItemQueryModel> _inventoryItemQueryModels = new();

        /// <summary>
        /// 재고아이템. 재고위치 테이블 데이터 소스.
        /// </summary>
        private readonly List<InventoryItemModel> _inventoryItemList = new();

        /// <summary>
        /// 수량합계 재고 아이템. 재고수량 테이블 데이터 소스.
        /// </summary>
        private readonly List<InventorySumItemModel> _inventorySumItemList = new();

        /// <summary>
        /// 현재 선택된 루트 위치그룹
        /// </summary>
        private LocationQueryModel? _selectedLocationGroup;

        /// <summary>
        /// 현재 선택된 수량합계 아이템
        /// </summary>
        private InventorySumItemModel? _focusedInventoryItemSum;


        private AidBlazor.AidTable<InventorySumItemModel>? _sumItemTable;

        /// <summary>
        /// 데이터 로딩중 여부
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// 접근 권한
        /// </summary>
        private bool _canAccess = false;

        private string? _searchText;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public InventoryItemApiClient InventoryApiClient { get; set; } = null!;
        [Inject] public LayoutApiClient LayoutApiClient { get; set; } = null!;
        [Inject] public ICanvasService CanvasService { get; set; } = null!;

        public int InventorySumItemTotal => _inventorySumItemList.Count;

        public int InventoryItemTotal => _inventoryItemList.Count;

        public string? SearchText
        {
            get => _searchText;
            set
            {
                if (EqualityComparer<string>.Default.Equals(_searchText, value))
                    return;
                _searchText = value;
                var filteredItems = _sumItemTable?.GetFilteredItems();
                FocusedInventorySumItem = filteredItems?.FirstOrDefault();
            }
        }

        public LocationQueryModel? SelectedLocationGroup
        {
            get => _selectedLocationGroup;
            set
            {
                if (EqualityComparer<LocationQueryModel>.Default.Equals(_selectedLocationGroup, value))
                    return;
                _selectedLocationGroup = value;

                _inventorySumItemList.Clear();
                _inventorySumItemList.AddRange(_itemQueryModels.Select(item =>
                    new InventorySumItemModel()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name,
                        // 루트 위치 조회
                        Quantity = _inventoryItemQueryModels.Where(x => x.ItemId == item.Id).Sum(x => x.Quantity)
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

        public InventorySumItemModel? FocusedInventorySumItem
        {
            get => _focusedInventoryItemSum;
            set
            {
                if (EqualityComparer<InventorySumItemModel>.Default.Equals(_focusedInventoryItemSum, value))
                    return;
                _focusedInventoryItemSum = value;

                if (value == null)
                {
                    _inventoryItemList.Clear();
                    CanvasService.SetBlink(new List<string>());
                    return;
                }

                _inventoryItemList.Clear();
                _inventoryItemList.AddRange(_inventoryItemQueryModels
                    .Where(x => x.ItemId == value.ItemId)
                    .Select(x => new InventoryItemModel()
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

                var itemLocationList = _inventoryItemQueryModels.Where(x => x.ItemId == value.ItemId).Select(x => x.LocationId);
                var layoutItemsToFlash = selectedLayout.ItemList
                    .Where(x => x.ConnectedLocations.Any(locId => itemLocationList.Contains(locId)));

                CanvasService.SetBlink(layoutItemsToFlash.Select(x => x.ItemId).ToList());
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _canAccess = true;
            await Load_Click();

            LoadDefaultValues();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CanvasService.InitCanvas(CANVAS_ID, Enumerable.Empty<PaletteItem>(), new CanvasCallbacks(), false, true);
                await CanvasService.Zoom(0.6);
            }
        }

        private bool FilterInventoryItem(InventorySumItemModel inventoryItem)
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                return true;
            if (inventoryItem == null)
                return false;

            return inventoryItem.ItemName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isLoading = true;

            var itemTask = ItemApiClient.GetItems();
            var locationTask = LocationApiClient.GetLocations();
            var inventoryTask = InventoryApiClient.GetInventoryDetails();
            var layoutTask = LayoutApiClient.GetLayouts();

            await Task.WhenAll(itemTask, locationTask, inventoryTask, layoutTask);

            var itemResponse = itemTask.Result;
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
            _rootLocationGroups.Clear();
            _layoutQueryModels.Clear();
            _inventoryItemQueryModels.Clear();

            _itemQueryModels.AddRange(itemResponse.Data);
            _locationQueryModels.AddRange(locationResponse.Data);
            _rootLocationGroups.AddRange(_locationQueryModels.Where(x => x.IsGroup && x.HierarchyLevel == 0));
            _layoutQueryModels.AddRange(layoutResponse.Data);
            _inventoryItemQueryModels.AddRange(inventoryResponse.Data);
 

            _isLoading = false;
        }

        /// <summary>
        /// 데이터 로딩 완료 후 기본값을 적용한다.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private void LoadDefaultValues()
        {
            SelectedLocationGroup = _rootLocationGroups.FirstOrDefault();
        }
    }
}