using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 사업장 내 구역
    /// </summary>
    public class WorkPlaceZone : CompanyEntity<long>
    {
        /// <summary>
        /// 구역명
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// 구역 유형
        /// </summary>
        public WorkPlaceZoneType? ZoneType { get; set; }

        /// <summary>
        /// 구역명을 변경한다.
        /// </summary>
        /// <param name="name">구역명</param>
        /// <exception cref="EmptyNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException("구역명이 비었습니다");
            Name = name.Trim();
        }

        /// <summary>
        /// 구역 유형을 변경한다.
        /// </summary>
        /// <param name="zoneType"></param>
        public void SetZoneType(WorkPlaceZoneType zoneType)
        {
            ZoneType = zoneType;
        }

    }
}
