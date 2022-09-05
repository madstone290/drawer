using DocumentFormat.OpenXml.EMMA;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using Drawer.Web.Pages.Item;
using System.Data;


namespace Drawer.Web.Pages.InventoryStatus.Models
{
    public class TreeNodeBuilder
    {
        private readonly List<ItemQueryModel> _items = new();

        private readonly List<LocationQueryModel> _locations = new();

        private readonly List<LocationGroupQueryModel> _groups = new();

        private readonly List<InventoryItemQueryModel> _inventoryItems = new();

        /// <summary>
        /// 노드 탐색(처리) 여부
        /// </summary>
        private readonly Dictionary<TreeNodeKey, bool> _handled = new();

        private readonly Dictionary<TreeNodeKey, TreeNode> _lookup = new();

        public TreeNodeBuilder(
            IEnumerable<ItemQueryModel> items, 
            IEnumerable<LocationGroupQueryModel> groups, 
            IEnumerable<LocationQueryModel> locations, 
            IEnumerable<InventoryItemQueryModel> inventoryItems)
        {
            _items.AddRange(items);
            _groups.AddRange(groups);
            _locations.AddRange(locations);
            _inventoryItems.AddRange(inventoryItems);
        }

        public IEnumerable<TreeNode> Build()
        {
            var treeNodes = BuildTree();

            foreach (var node in treeNodes)
                node.Visible = true;

            return treeNodes;
        }

        private IEnumerable<TreeNode> BuildTree()
        {
            // 아이템 노드(루트노드)
            foreach (var item in _items)
            {
                var rootNode = new TreeNode()
                {
                    Key = new TreeNodeKey()
                    {
                        ItemId = item.Id,
                    },
                    InventoryItem = new ItemQtyLocationModel()
                    {
                        ItemId = item.Id,
                        ItemName = item.Name,
                    }
                };
                _lookup.Add(rootNode.Key, rootNode);
            }
             
            // 위치그룹 노드 생성
            foreach (var item in _items)
            {
                foreach (var group in _groups.OrderBy(x => x.Depth)) 
                {
                    var node = new TreeNode()
                    {
                        Key = new TreeNodeKey()
                        {
                            ItemId = item.Id,
                            GroupId = group.Id,
                        },
                        InventoryItem = new ItemQtyLocationModel()
                        {
                            ItemId = item.Id,
                            ItemName = item.Name,
                            GroupId = group.Id,
                            LocationName = group.Name,
                        }
                    };

                    // 부모그룹을 포함하는 노드 검색
                    // 위치그룹 상하관계가 노드에 동일하게 적용된다.
                    var parentNode = _lookup.Values.First(x => 
                        x.Key.ItemId == item.Id &&
                        x.Key.GroupId == group.ParentGroupId);

                    parentNode.Children.Add(node);
                    node.Parent = parentNode;

                    _lookup.Add(node.Key, node);
                }
            }

            // 위치 노드 생성. 위치의 그룹이 포함된 노드를 검색한 후 부모노드로 설정한다.
            foreach(var invenItem in _inventoryItems)
            {
                var node = new TreeNode()
                {
                    Key = new TreeNodeKey()
                    {
                        ItemId = invenItem.ItemId,
                        LocationId = invenItem.LocationId
                    },
                    InventoryItem = new ItemQtyLocationModel()
                    {
                        ItemId = invenItem.ItemId,
                        ItemName = _items.First(x => x.Id == invenItem.ItemId).Name,
                        LocationId = invenItem.LocationId,
                        LocationName = _locations.First(x => x.Id == invenItem.LocationId).Name,
                        Quantity = invenItem.Quantity
                    }
                };

                var groupId = GetGroupId(invenItem.LocationId);
                var parentNode = _lookup.Values.First(x => x.Key.ItemId == invenItem.ItemId && x.Key.GroupId == groupId);

                parentNode.Children.Add(node);
                node.Parent = parentNode;

                parentNode.AddQuantity(node.InventoryItem.Quantity);
            }
            
            return _lookup.Values.Where(x => x.Parent == null);
        }

        long GetGroupId(long locationId)
        {
            var location = _locations.First(x => x.Id == locationId);
            return location.GroupId;
        }
       

    }
}
