using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Domain.Models.Inventory;
using System.Data;


namespace Drawer.Web.Pages.InventoryStatus.Models
{
    public class TreeNodeBuilder
    {
        private readonly List<ItemQueryModel> _items = new List<ItemQueryModel>();
        private readonly List<LocationQueryModel> _locations = new List<LocationQueryModel>();
        /// <summary>
        /// 노드 탐색(처리) 여부
        /// </summary>
        private readonly Dictionary<TreeNodeKey, bool> handled = new Dictionary<TreeNodeKey, bool>();

        public IEnumerable<TreeNode> Build(
            IEnumerable<ItemQueryModel> itemQueryModels,
            IEnumerable<LocationQueryModel> locationQueryModels, 
            IEnumerable<InventoryItemQueryModel> inventoryItemQueryModels)
        {
            _items.Clear();
            _locations.Clear();

            _items.AddRange(itemQueryModels);
            _locations.AddRange(locationQueryModels);

            // 전체 아이템 목록
            var defaultItems = itemQueryModels.Select(item => new InventoryItemModel()
            {
                ItemId = item.Id,
                ItemName = item.Name
            });

            // 재고등록된 아이템 목록
            var inventoryItems = inventoryItemQueryModels.Select(inventoryItem => new InventoryItemModel()
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


    

        public IEnumerable<TreeNode> Build(IEnumerable<InventoryItemModel> inventoryItems)
        {
            Dictionary<TreeNodeKey, TreeNode> lookup = new Dictionary<TreeNodeKey, TreeNode>();
            foreach (var item in inventoryItems)
            {
                var node = new TreeNode()
                {
                    Key = new TreeNodeKey() { ItemId = item.ItemId, LocationId = item.LocationId },
                    InventoryItem = item
                };

                lookup.Add(node.Key, node);
                Fill(lookup, node);
            }
            return lookup.Values.Where(x => x.Parent == null);
        }

        void Fill(Dictionary<TreeNodeKey, TreeNode> lookup, TreeNode node)
        {
            // 서로 다른 노드라 하더라도 부모 노드는 동일한 경우가 발생할 수 있으므로 모든 노드는 1번만 처리한다.

            handled[node.Key] = true;
            
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
                    InventoryItem = new InventoryItemModel()
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
            if (handled.GetValueOrDefault(parentKey))
                return;

            Fill(lookup, parentNode);
        }


        long GetParentLocationId(long locationId)
        {
            var location = _locations.First(x => x.Id == locationId);
            return location.ParentGroupId ?? 0;
        }

    }
}
