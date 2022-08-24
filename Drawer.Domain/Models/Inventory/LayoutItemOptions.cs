using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    public class LayoutItemOptions
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
