using DocumentFormat.OpenXml.Office2013.PowerPoint;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus.Models
{
    public struct TreeNodeKey
    {
        public long ItemId { get; set; }
        public long LocationId { get; set; }
        public override string ToString()
        {
            return $"{ItemId}   {LocationId}";
        }
    }

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
