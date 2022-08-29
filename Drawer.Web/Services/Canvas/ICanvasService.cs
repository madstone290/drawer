using Microsoft.JSInterop;

namespace Drawer.Web.Services.Canvas
{
    /// <summary>
    /// Html Canvas 기능을 제공한다.
    /// </summary>
    public interface ICanvasService
    {
        Task InitCanvas(string canvasId, IEnumerable<PaletteItem> paletteItems, CanvasCallbacks callbacks, bool drawGridLines);
        Task SetBackColor(string id, string? colorCode);
        Task SetDegree(string id, string degree);
        Task SetHAlignment(string id, string horizontalAlignment);
        Task SetText(string id, string text);
        Task SetVAlignment(string id, string verticalAlignment);
        Task<CanvasItem?> GetItem(string id);
        Task DeleteItem(string id);
        Task ClearCanvas();
        Task<List<CanvasItem>> ExportItemList();
        Task ImportItemList(List<CanvasItem> result);
        Task SetInteraction(bool enabled);
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

        public async Task InitCanvas(string canvasId, IEnumerable<PaletteItem> paletteItems, CanvasCallbacks callbacks, bool drawGridLines)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
            _canvasCallbacks = DotNetObjectReference.Create(callbacks);
            await _module.InvokeVoidAsync("initDrawer", _canvasCallbacks, canvasId, paletteItems);

            if(drawGridLines)
                await _module.InvokeVoidAsync("drawGridLines");
        }

        public async Task SetBackColor(string id, string? colorCode)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setBackColor", id, colorCode);
        }

        public async Task SetText(string id, string text)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setText", id, text);
        }

        public async Task SetVAlignment(string id, string verticalAlignment)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setVAlignment", id, verticalAlignment);
        }


        public async Task SetHAlignment(string id, string horizontalAlignment)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setHAlignment", id, horizontalAlignment);
        }

        public async Task SetDegree(string id, string degree)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setDegree", id, degree);
        }


        public async Task<CanvasItem?> GetItem(string id)
        {
            if (_module == null)
                return null;
            return await _module.InvokeAsync<CanvasItem>("getItemInfo", id);
        }

        public async Task DeleteItem(string id)
        {
            if (_module == null)
                return;
            await _module.InvokeAsync<CanvasItem>("deleteItem", id);
        }

        public async Task ClearCanvas()
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("clearCanvas");
        }

        public async Task<List<CanvasItem>> ExportItemList()
        {
            if (_module == null)
                return new List<CanvasItem>();
            return await _module.InvokeAsync<List<CanvasItem>>("exportItemList");
        }

        public async Task ImportItemList(List<CanvasItem> itemList)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("importItemList", itemList);
        }

        public async Task SetInteraction(bool enabled)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync("setInteraction", enabled);
        }
    }

}
