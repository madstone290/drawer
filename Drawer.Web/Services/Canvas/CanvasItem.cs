namespace Drawer.Web.Services.Canvas
{
    /// <summary>
    /// 캔버스에 포함된 아이템
    /// </summary>
    public class CanvasItem
    {
        public string ItemId { get; set; } = string.Empty;

        public string Type { get; set; } = "group";
        public string Shape { get; set; } = "rect";

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;

        public bool IsPattern { get; set; }
        public string? BackColor { get; set; } = "#ADD8E6";
        public string? PatternImageId { get; set; }

        public string Text { get; set; } = string.Empty;
        public int FontSize { get; set; } = 15;
        public string Degree { get; set; } = Options.Degree.Row;
        public string VAlignment { get; set; } = Options.VAlignment.Center;
        public string HAlignment { get; set; } = Options.HAlignment.Center;


        public class Options
        {
            public class Shape
            {
                public const string Rect = "rect";
                public const string Circle = "circle";
                public static IEnumerable<string> Collection { get; } = new string[] { Rect, Circle };
            }
            public class Degree
            {
                public const string Row = "row";
                public const string Column = "column";
                public static IEnumerable<string> Collection { get; } = new string[] { Row, Column };
            }
            public class VAlignment
            {
                public const string Top = "top";
                public const string Center = "center";
                public const string Bottom = "bottom";
                public static IEnumerable<string> Collection { get; } = new string[] { Top, Center, Bottom };
            }

            public class HAlignment
            {
                public const string Left = "left";
                public const string Center = "center";
                public const string Right = "right";
                public static IEnumerable<string> Collection { get; } = new string[] { Left, Center, Right };
            }
        }
    }
}
