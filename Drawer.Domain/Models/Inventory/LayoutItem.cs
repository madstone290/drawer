using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    /// <summary>
    /// 배치도를 구성하는 항목
    /// </summary>
    public class LayoutItem : ValueObject
    {
        public IEnumerable<long> ConnectedLocations
        {
            get
            {
                return ConnectedLocationsString?.Split(",").Select(x => Convert.ToInt64(x)) ?? Enumerable.Empty<long>();
            }
            set
            {
                if (value == null)
                    ConnectedLocationsString = null;
                else
                    ConnectedLocationsString = string.Join(",", value.Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// 연결된 위치들의 ID 목록.
        /// 콤마로 구분한다.
        /// </summary>
        public string? ConnectedLocationsString { get; set; }

        /// <summary>
        /// 캔버스 아이템 식별자
        /// </summary>
        public string ItemId { get; set; } = null!;

        /// <summary>
        /// 아이템 모양.
        /// rect, circle
        /// </summary>
        public string Shape { get; set; } = LayoutItemOptions.Shape.Rect;

        public int Left { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;

        public bool IsPattern { get; set; } = false;
        public string? BackColor { get; set; } = "#ADD8E6";
        public string? PatternImageId { get; set; } = null;

        public string Text { get; set; } = string.Empty;
        public int FontSize { get; set; } = 15;
        public string Degree { get; set; } = LayoutItemOptions.Degree.Row;
        public string VAlignment { get; set; } = LayoutItemOptions.VAlignment.Center;
        public string HAlignment { get; set; } = LayoutItemOptions.HAlignment.Center;

        /// <summary>
        /// 속성 값이 유효한지 검사한다.
        /// 유효한 경우 null을 반환하고 아닌 경우 에러내용을 반환한다.
        /// </summary>
        /// <returns></returns>
        public string? Validate()
        {
            if (!LayoutItemOptions.Shape.Collection.Contains(Shape))
                return "아이템 모양이 유효하지 않습니다";
            if (!LayoutItemOptions.Degree.Collection.Contains(Degree))
                return "각도가 유효하지 않습니다";
            if (!LayoutItemOptions.VAlignment.Collection.Contains(VAlignment))
                return "수직 정렬이 유효하지 않습니다";
            if (!LayoutItemOptions.HAlignment.Collection.Contains(HAlignment))
                return "수평 정렬이 유효하지 않습니다";
            if (ConnectedLocationsString != null && ConnectedLocationsString.Split(",").Any(str => !long.TryParse(str, out long _)))
                return "위치목록이 유효하지 않습니다";

            return null;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ConnectedLocationsString;
            yield return ItemId;
            yield return Shape;
            yield return Left;
            yield return Top;
            yield return Width;
            yield return Height;
            yield return IsPattern;
            yield return BackColor;
            yield return PatternImageId;
            yield return Text;
            yield return FontSize;
            yield return Degree;
            yield return VAlignment;
            yield return HAlignment;
        }
    }
}
