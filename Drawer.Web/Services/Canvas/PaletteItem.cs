namespace Drawer.Web.Services.Canvas
{
    /// <summary>
    /// 캔버스 팔레트 영역의 아이템
    /// </summary>
    public class PaletteItem
    {
        public string ElementId { get; set; }
        public string Shape { get; set; }
        public string? ImageSrc { get; set; }

        public static PaletteItem Rect(string elementId)
        {
            return new PaletteItem(elementId, CanvasItem.Options.Shape.Rect);
        }

        public static PaletteItem Circle(string elementId)
        {
            return new PaletteItem(elementId, CanvasItem.Options.Shape.Circle);
        }

        private PaletteItem(string elementId, string shape)
        {
            ElementId = elementId;
            Shape = shape;
        }
    }
}
