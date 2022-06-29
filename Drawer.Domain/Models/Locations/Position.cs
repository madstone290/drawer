using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 구역 내 위치
    /// </summary>
    public class Position : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 위치명
        /// </summary>
        public string Name { get; private set; } = default!;

        public Position(string name) 
        {
            SetName(name);
        }

        /// <summary>
        /// 위치명을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="EmptyNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException("위치명이 비었습니다");
            Name = name.Trim();
        }
    }
}
