using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    public class Item : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 품명
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// 코드
        /// </summary>
        public string? Code { get; private set; }

        /// <summary>
        /// 품번
        /// </summary>
        public string? Number { get; private set; }

        /// <summary>
        /// 재고관리단위
        /// </summary>
        public string? Sku { get; private set; }

        /// <summary>
        /// 수량 단위
        /// </summary>
        public string? QuantityUnit { get; private set; }

        private Item() { }
        public Item(string name)
        {
            SetName(name);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException();
            Name = name.Trim();
        }

        public void SetCode(string? code)
        {
            Code = code?.Trim();
        }

        public void SetNumber(string? number)
        {
            Number = number?.Trim();
        }

        public void SetSku(string? sku)
        {
            Sku = sku?.Trim();
        }

        public void SetQuantityUnit(string? quantityUnit)
        {
            QuantityUnit = quantityUnit?.Trim();
        }
    }
}
