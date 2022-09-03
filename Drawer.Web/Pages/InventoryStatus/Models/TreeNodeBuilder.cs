using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System.Data;


namespace Drawer.Web.Pages.InventoryStatus.Models
{
    public class TreeNodeBuilder
    {
        private readonly List<ItemQueryModel> _items = new();

        private readonly List<LocationQueryModel> _locations = new();

        private readonly List<InventoryItemQueryModel> _inventoryItems = new();

        /// <summary>
        /// 노드 탐색(처리) 여부
        /// </summary>
        private readonly Dictionary<TreeNodeKey, bool> _handled = new();

        private readonly Dictionary<TreeNodeKey, TreeNode> _lookup = new();

        public TreeNodeBuilder(IEnumerable<ItemQueryModel> items, IEnumerable<LocationQueryModel> locations, IEnumerable<InventoryItemQueryModel> inventoryItems)
        {
            _items.AddRange(items);
            _locations.AddRange(locations);
            _inventoryItems.AddRange(inventoryItems);
        }

        public IEnumerable<TreeNode> Build()
        {
            // 전체 아이템 목록
            var defaultItems = _items.Select(item => new ItemQtyLocationModel()
            {
                ItemId = item.Id,
                ItemName = item.Name
            });

            // 재고등록된 아이템 목록
            var inventoryItems = _inventoryItems.Select(inventoryItem => new ItemQtyLocationModel()
            {
                ItemId = inventoryItem.ItemId,
                ItemName = _items.First(x=> x.Id == inventoryItem.ItemId).Name,
                Quantity = inventoryItem.Quantity,
                LocationId = inventoryItem.LocationId,
                LocationName = _locations.First(x=> x.Id == inventoryItem.LocationId).Name
            });
            
            var nodes = Build(defaultItems.Concat(inventoryItems));

            foreach (var node in nodes)
                node.Visible = true;

            return nodes;
        }

        private IEnumerable<TreeNode> Build(IEnumerable<ItemQtyLocationModel> inventoryItems)
        {
           
            foreach (var item in inventoryItems)
            {
                var node = new TreeNode()
                {
                    Key = new TreeNodeKey() { ItemId = item.ItemId, LocationId = item.LocationId },
                    InventoryItem = item
                };

                _lookup.Add(node.Key, node);
                FillLookup(_lookup, node);
            }
            return _lookup.Values.Where(x => x.Parent == null);
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
            
            // LocationId가 0인 것은 루트 노드. 
            if (node.Key.LocationId == 0)
                return;

            var parentKey = new TreeNodeKey() { ItemId = node.Key.ItemId, LocationId = GetParentLocationId(node.Key.LocationId) };
            lookup.TryGetValue(parentKey, out TreeNode? parentNode);

            if(parentNode == null)
            {
                parentNode = new TreeNode()
                {
                    Key = parentKey,
                    InventoryItem = new ItemQtyLocationModel()
                    {
                        ItemId = parentKey.ItemId,
                        LocationId = parentKey.LocationId,
                        ItemName = _items.First(x=> x.Id == parentKey.ItemId).Name,
                        LocationName = _locations.FirstOrDefault(x=> x.Id == parentKey.LocationId)?.Name,
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


        long GetParentLocationId(long locationId)
        {
            var location = _locations.First(x => x.Id == locationId);
            return location.ParentGroupId ?? 0;
        }

    }
}
