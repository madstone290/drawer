using Drawer.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    /// <summary>
    /// 위치의 레이아웃을 관리한다.
    /// 위치그룹만 레이아웃을 가질 수 있다.
    /// </summary>
    public class Layout : CompanyResourceEntity<long>
    {
        private readonly ISet<LayoutItem> _items = new HashSet<LayoutItem>();

        private Layout() { }
        public Layout(long locationGroupId)
        {
            LocationGroupId = locationGroupId;
        }

        /// <summary>
        /// 위치그룹 ID
        /// </summary>
        public long LocationGroupId { get; private set; }

        /// <summary>
        /// 레이아웃에 포함된 아이템
        /// </summary>
        public IEnumerable<LayoutItem> Items => _items;

        /// <summary>
        /// 레이아웃을 일괄 업데이트 한다.
        /// </summary>
        /// <param name="layoutItems"></param>
        public void Update(IEnumerable<LayoutItem> layoutItems)
        {
            _items.Clear();
            foreach (var item in layoutItems)
            {
                var message = item.Validate();
                if (message != null)
                    throw new DomainException(message);

                _items.Add(item);
            }
        }

    }
}
