using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Layout.Models;
using Drawer.Web.Pages.Location.Models;
using Drawer.Web.Services;
using Drawer.Web.Services.Canvas;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Layout
{
    public partial class LayoutEdit
    {

        private const string DISPLAY_NONE = "display:none";

        /// <summary>
        /// 데이터 로드 태스크. 렌더링 후 로드완료 상태를 확인해야한다.
        /// </summary>
        private Task _loadTask;

        private bool _canAccess = true;
        private string? _displayStyle = DISPLAY_NONE;
        private string? _selectedCanvasItemId;
        private readonly LayoutModel _layout = new LayoutModel();

        private bool _propertyPanelVisible = true;
        public bool PropertyPanelVisible
        {
            get => _propertyPanelVisible;
            set
            {
                _propertyPanelVisible = value;
                if (value)
                    _displayStyle = null;
                else
                    _displayStyle = DISPLAY_NONE;
            }
        }



        private string? _backColor;
        public string? BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                if (_selectedCanvasItemId != null)
                    _ = CanvasService.SetBackColor(_selectedCanvasItemId, value);
            }
        }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (_selectedCanvasItemId != null)
                    _ = CanvasService.SetText(_selectedCanvasItemId, value);
            }
        }

        private string _vAlignment = CanvasItem.Options.VAlignment.Top;
        public string VAlignment
        {
            get => _vAlignment;
            set
            {
                _vAlignment = value;
                if (_selectedCanvasItemId != null)
                    _ = CanvasService.SetVAlignment(_selectedCanvasItemId, value);
            }
        }

        private string _hAlignment = CanvasItem.Options.HAlignment.Center;
        public string HAlignment
        {
            get => _hAlignment;
            set
            {
                _hAlignment = value;
                if (_selectedCanvasItemId != null)
                    _ = CanvasService.SetHAlignment(_selectedCanvasItemId, value);
            }
        }

        private string _degree = CanvasItem.Options.Degree.Row;
        public string Degree
        {
            get => _degree;
            set
            {
                _degree = value;
                if (_selectedCanvasItemId != null)
                    _ = CanvasService.SetDegree(_selectedCanvasItemId, value);
            }
        }


        [Parameter]
        [SupplyParameterFromQuery]
        public string LocationId { get; set; } = null!;

        [Inject] public ICanvasService CanvasService { get; set; } = null!;
        [Inject] public LayoutApiClient LayoutApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _loadTask = LoadLayout();
            await _loadTask;
        }

        async Task LoadLayout()
        {
            if (!long.TryParse(LocationId, out long locationId))
            {
                Snackbar.Add("레이아웃을 찾을 수 없습니다", Severity.Error);
                return;
            }

            var response = await LayoutApiClient.GetLayoutByLocation(locationId);
            if (!Snackbar.CheckFail(response))
                return;

            if (response.Data == null)
            {
                // 임시
                _layout.LocationId = locationId;
                _layout.ItemList = new List<Domain.Models.Inventory.LayoutItem>();
            }
            else
            {
                _layout.LocationId = locationId;
                _layout.ItemList = response.Data.ItemList;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                /** 캔버스 아이템을 추가하기 위해선 렌더링이 완료되어야 한다. **/

                if (!long.TryParse(LocationId, out long locationId))
                {
                    Snackbar.Add("레이아웃을 찾을 수 없습니다", Severity.Error);
                    return;
                }

                var response = await LayoutApiClient.GetLayoutByLocation(locationId);
                if (!Snackbar.CheckFail(response))
                    return;

                if (response.Data == null)
                {
                    _layout.LocationId = locationId;
                    _layout.ItemList = new List<Domain.Models.Inventory.LayoutItem>();
                }
                else
                {
                    _layout.LocationId = locationId;
                    _layout.ItemList = response.Data.ItemList;
                }

                var canvasId = "canvas";
                var paletteItems = new PaletteItem[]
                {
                    PaletteItem.Rect("rectItemImage"),
                    PaletteItem.Circle("circleItemImage"),
                };
                var canvasMediator = new CanvasCallbacks();
                canvasMediator.OnItemSelectionChanged = new EventCallback<string>(this, UpdateText);
                await CanvasService.InitCanvas(canvasId, paletteItems, canvasMediator);

                await _loadTask;

                await CanvasService.ImportItemList(
                    _layout.ItemList.Select(x => CanvasItemConverter.ToCanvasItem(x)).ToList());
            }
        }


        async Task Save_ClickAsync()
        {
            _layout.ItemList = (await CanvasService.ExportItemList()).Select(x => new Domain.Models.Inventory.LayoutItem()
            {
                ItemId = x.ItemId,

                Shape = x.Shape,
                Top = x.Top,
                Left = x.Left,
                Width = x.Width,
                Height = x.Height,
                IsPattern = x.IsPattern,
                BackColor = x.BackColor,
                PatternImageId = x.PatternImageId,

                Text = x.Text,
                FontSize = x.FontSize,
                Degree = x.Degree,
                HAlignment = x.HAlignment,
                VAlignment = x.VAlignment,
            }).ToList();

            await LayoutApiClient.EditLayout(new LayoutEditCommandModel()
            {
                LocationId = _layout.LocationId,
                ItemList = _layout.ItemList
            });
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.LayoutHome);
        }

        async void UpdateText(string id)
        {
            _selectedCanvasItemId = id;

            // update status items
            var itemInfo = await CanvasService.GetItem(id);

            if (itemInfo == null)
            {
                PropertyPanelVisible = false;
            }
            else
            {
                PropertyPanelVisible = true;

                _backColor = itemInfo.BackColor;

                _text = itemInfo.Text;
                _degree = itemInfo.Degree;
                _hAlignment = itemInfo.HAlignment;
                _vAlignment = itemInfo.VAlignment;
            }
            StateHasChanged();
        }


        List<CanvasItem> _itemInfos = new List<CanvasItem>();
        async Task Export_Click()
        {
            _itemInfos.Clear();
            var itemInfos = await CanvasService.ExportItemList();
            _itemInfos.AddRange(itemInfos);
        }

        async Task Import_ClickAsync()
        {
            await CanvasService.ImportItemList(_itemInfos);
        }

    }
}
