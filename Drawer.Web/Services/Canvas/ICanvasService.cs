using Microsoft.JSInterop;

namespace Drawer.Web.Services.Canvas
{
    /// <summary>
    /// Html Canvas 기능을 제공한다.
    /// </summary>
    public interface ICanvasService
    {
        Task InitCanvas(string canvasId, IEnumerable<PaletteItem> paletteItems, CanvasCallbacks callbacks, bool drawGridLines, bool useInterval = false);
        Task SetBackColor(string id, string? colorCode);
        Task SetDegree(string id, string degree);
        Task SetHAlignment(string id, string horizontalAlignment);
        Task SetText(string id, string text);
        Task SetFontSize(string id, int fontSize);
        Task SetVAlignment(string id, string verticalAlignment);
        Task<CanvasItem?> GetItem(string id);
        Task DeleteItem(string id);
        Task ClearCanvas();
        Task<List<CanvasItem>> ExportItemList();
        Task ImportItemList(List<CanvasItem> result);
        Task SetInteraction(bool enabled);
        Task Zoom(double level);
        /// <summary>
        /// 캔버스 아이템이 깜빡이도록 한다.
        /// </summary>
        /// <param name="itemIdList"></param>
        /// <returns></returns>
        Task SetBlink(List<string> itemIdList);

        Task DisposeAsync();
    }

    public class CanvasService : ICanvasService
    {
        private const string JS_FILE = "./js/canvas-helper.js";

        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<CanvasCallbacks>? _canvasCallbacks;
        private IJSObjectReference? _module;

        public CanvasService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task InvokeVoidAsync(string identifier, params object?[]? args)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync(identifier, args);
        }

        private async Task<T?> InvokeAsync<T>(string identifier, params object?[]? args)
        {
            if (_module == null)
                return default;
            return await _module.InvokeAsync<T>(identifier, args);
        }

        public async Task InitCanvas(string canvasId, IEnumerable<PaletteItem> paletteItems, CanvasCallbacks callbacks, bool drawGridLines, bool useInterval)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
            _canvasCallbacks = DotNetObjectReference.Create(callbacks);

            await InvokeVoidAsync("initDrawer", _canvasCallbacks, canvasId, paletteItems, useInterval);

            if(drawGridLines)
                await InvokeVoidAsync("drawGridLines");
        }

        public async Task SetBackColor(string id, string? colorCode)
        {
            await InvokeVoidAsync("setBackColor", id, colorCode);
        }

        public async Task SetText(string id, string text)
        {
            await InvokeVoidAsync("setText", id, text);
        }

        public async Task SetFontSize(string id, int fontSize)
        {
            await InvokeVoidAsync("setFontSize", id, fontSize);
        }

        public async Task SetVAlignment(string id, string verticalAlignment)
        {
            await InvokeVoidAsync("setVAlignment", id, verticalAlignment);
        }


        public async Task SetHAlignment(string id, string horizontalAlignment)
        {
            await InvokeVoidAsync("setHAlignment", id, horizontalAlignment);
        }

        public async Task SetDegree(string id, string degree)
        {
            await InvokeVoidAsync("setDegree", id, degree);
        }

        public async Task<CanvasItem?> GetItem(string id)
        {
            return await InvokeAsync<CanvasItem>("getItemInfo", id);
        }

        public async Task DeleteItem(string id)
        {
            await InvokeAsync<CanvasItem>("deleteItem", id);
        }

        public async Task ClearCanvas()
        {
            await InvokeVoidAsync("clearCanvas");
        }

        public async Task<List<CanvasItem>> ExportItemList()
        {
            var canvasItemList = await InvokeAsync<List<CanvasItem>>("exportItemList");
            return canvasItemList ?? new List<CanvasItem>();
        }

        public async Task ImportItemList(List<CanvasItem> itemList)
        {
            await InvokeVoidAsync("importItemList", itemList);
        }

        public async Task SetInteraction(bool enabled)
        {
            await InvokeVoidAsync("setInteraction", enabled);
        }

        public async Task Zoom(double level)
        {
            await InvokeVoidAsync("zoom", level);
        }

        public async Task SetBlink(List<string> itemIdList)
        {
            await InvokeVoidAsync("setBlink", itemIdList);
        }

        public async Task DisposeAsync()
        {
            if(_module != null)
            {
                await _module.InvokeVoidAsync("dispose");
                await _module.DisposeAsync();
            }
                
        }
    }

}
