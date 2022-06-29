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

        /// <summary>
        /// 구역
        /// </summary>
        public Zone Zone { get; private set; } = null!;

        public long ZoneId { get; private set; }

        private Position() { }
        public Position(Zone zone, string name) 
        {
            if (zone == null)
                throw new EntityNullException<Zone>(nameof(zone));
            Zone = zone;
            ZoneId = zone.Id;
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
