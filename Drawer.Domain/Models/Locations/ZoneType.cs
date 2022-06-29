using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 구역 유형.
    /// 구역 유형별로 재고를 제공한다.
    /// </summary>
    public class ZoneType : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 유형명
        /// </summary>
        public string Name { get; private set; } = null!;

        public ZoneType(string name)
        {
            SetName(name);
        }

        /// <summary>
        /// 구역 유형명을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="EmptyNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException("유형명이 비었습니다");
            Name = name.Trim();
        }
    }
}
