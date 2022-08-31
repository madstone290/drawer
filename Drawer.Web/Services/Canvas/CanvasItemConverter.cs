using Drawer.Domain.Models.Inventory;

namespace Drawer.Web.Services.Canvas
{
    public static class CanvasItemConverter
    {
        public static CanvasItem ToCanvasItem(LayoutItem layoutItem)
        {
            return new CanvasItem()
            {
                ItemId = layoutItem.ItemId,

                Shape = layoutItem.Shape,
                Top = layoutItem.Top,
                Left = layoutItem.Left,
                Width = layoutItem.Width,
                Height = layoutItem.Height,
                IsPattern = layoutItem.IsPattern,
                BackColor = layoutItem.BackColor,
                PatternImageId = layoutItem.PatternImageId,

                Text = layoutItem.Text,
                FontSize = layoutItem.FontSize,
                Degree = layoutItem.Degree,
                HAlignment = layoutItem.HAlignment,
                VAlignment = layoutItem.VAlignment,
            };
        }

        public static LayoutItem ToLayoutItem(CanvasItem canvasItem)
        {

            return new LayoutItem()
            {
                ItemId = canvasItem.ItemId,

                Shape = canvasItem.Shape,
                Top = canvasItem.Top,
                Left = canvasItem.Left,
                Width = canvasItem.Width,
                Height = canvasItem.Height,
                IsPattern = canvasItem.IsPattern,
                BackColor = canvasItem.BackColor,
                PatternImageId = canvasItem.PatternImageId,

                Text = canvasItem.Text,
                FontSize = canvasItem.FontSize,
                Degree = canvasItem.Degree,
                HAlignment = canvasItem.HAlignment,
                VAlignment = canvasItem.VAlignment,
            };
        }


    }
}
