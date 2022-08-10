using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.CommandModels
{
    public class ItemAddUpdateCommandModel
    {
        /// <summary>
        /// 품명
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 코드
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 품번
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// 재고관리단위
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// 수량 단위
        /// </summary>
        public string? QuantityUnit { get; set; }
    }


}
