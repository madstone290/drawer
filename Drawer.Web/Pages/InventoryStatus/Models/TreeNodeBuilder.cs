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
            foreach(var item in _items)
            {
                foreach(var group in _groups.OrderBy(x=> x.Depth))
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

                    var parentNode = _lookup.Values.FirstOrDefault(x => x.Key.GroupId == group.ParentGroupId);
                    if(parentNode != null)
                    {
                        parentNode.Children.Add(node);
                        node.Parent = parentNode;
                    }

                    _lookup.Add(node.Key, node);
                }
            }

            foreach(var invenItem in _inventoryItems)
            {
                var groupId = GetGroupId(invenItem.LocationId);
                var node = _lookup.Values.FirstOrDefault(x => x.Key.ItemId == invenItem.ItemId && x.Key.GroupId == groupId);
                if(node != null)
                {
                    node.Children.Add(new TreeNode()
                    {
                        Key = new TreeNodeKey()
                        {
                            ItemId = invenItem.ItemId,
                            GroupId = groupId,
                            LocationId = invenItem.LocationId
                        },
                        InventoryItem = new ItemQtyLocationModel()
                        {
                            ItemId = invenItem.ItemId,
                            ItemName = _items.First(x=> x.Id == invenItem.ItemId).Name,
                            LocationId = invenItem.LocationId,
                            LocationName = _locations.First(x=> x.Id == invenItem.LocationId).Name,
                        }
                    });
                    node.InventoryItem.Quantity += invenItem.Quantity;
                }
            }
            
            return _lookup.Values.Where(x => x.Parent == null);
        }

        long GetGroupId(long locationId)
        {
            var location = _locations.First(x => x.Id == locationId);
            return location.GroupId;
        }

        /// <summary>
        /// 부모노드를 탐색 후 존재하지 않은 경우 부모노드를 생성하고 룩업 딕셔너리를 채운다.
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="node"></param>
        void FillLookup(Dictionary<TreeNodeKey, TreeNode> lookup, TreeNode node)
        {
            // 서로 다른 노드라 하더라도 부모 노드는 동일한 경우가 발생할 수 있으므로 모든 노드는 1번만 처리한다.

            _handled[node.Key] = true;
            
            // GroupId가 0인 것은 루트 노드. 
            if (node.Key.GroupId == 0)
                return;

            // 위치가 아닌 그룹에 대한 재고정보를 생성한다.
            var parentKey = new TreeNodeKey() 
            { 
                ItemId = node.Key.ItemId, 
                GroupId = GetGroupId(node.Key.LocationId)
            };
            lookup.TryGetValue(parentKey, out TreeNode? parentNode);

            if(parentNode == null)
            {
                parentNode = new TreeNode()
                {
                    Key = parentKey,
                    InventoryItem = new ItemQtyLocationModel()
                    {
                        ItemId = parentKey.ItemId,
                        GroupId = parentKey.GroupId,
                        ItemName = _items.First(x=> x.Id == parentKey.ItemId).Name,
                        LocationName = _groups.FirstOrDefault(x=> x.Id == parentKey.GroupId)?.Name,
                    }
                };
                lookup.Add(parentKey, parentNode);
            }
            
            node.Parent = parentNode;
            parentNode.Children.Add(node);
            

            parentNode.AddQuantity(node.InventoryItem.Quantity);


            // 노드 탐색이 두번이상 발생하지 않도록 한다.
            if (_handled.GetValueOrDefault(parentKey))
                return;

            FillLookup(lookup, parentNode);
        }


       

    }
}
