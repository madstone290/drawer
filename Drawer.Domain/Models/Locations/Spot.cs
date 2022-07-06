using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 구역 내 자리
    /// </summary>
    public class Spot : CompanyResourceEntity<long>
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

        /// <summary>
        /// 비고
        /// </summary>
        public string? Note { get; private set; }

        private Spot() { }
        public Spot(Zone zone, string name) 
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
                throw new EmptyNameException("자리 이름이 비었습니다");
            Name = name.Trim();
        }

        /// <summary>
        /// 비고를 변경한다.
        /// </summary>
        /// <param name="note"></param>
        public void SetNote(string? note)
        {
            Note = note?.Trim();
        }
    }
}
