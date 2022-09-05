using DocumentFormat.OpenXml.Office2013.PowerPoint;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus.Models
{
    /// <summary>
    /// 노드는 아이템/그룹/위치 총 3가지 종류가 있다.
    /// </summary>
    public struct TreeNodeKey
    {
        /// <summary>
        /// 아이템 ID. 
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// 그룹노드인 경우 그룹의ID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// 위치 노드인 경우 위치의ID
        /// </summary>
        public long? LocationId { get; set; }
    }

    /// <summary>
    /// 위치 혹은 위치그룹에 존재하는 재고수량을 트리로 표현한다.
    /// </summary>
    public class TreeNode
    {
        public TreeNodeKey Key { get; set; }

        public TreeNode Root => Parent?.Root ?? this;
        public TreeNode? Parent { get; set; }
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();

        public ItemQtyLocationModel InventoryItem { get; set; } = null!;

        public bool Visible { get; set; }

        public bool Expanded { get; set; }

        /// <summary>
        /// 노드레벨. 0부터 시작.
        /// </summary>
        public int Level { get; set; }
        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children.Count == 0;

        public string Style => IsLeaf
         ? "visibility:hidden"
         : string.Empty;


        public string Icon => Expanded
            ? Icons.Material.Filled.ExpandLess
            : Icons.Material.Filled.ExpandMore;

        public void AddQuantity(decimal quantity)
        {
            InventoryItem.Quantity += quantity;
            if (Parent != null)
                Parent.AddQuantity(quantity);
        }

    }
}
