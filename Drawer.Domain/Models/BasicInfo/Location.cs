using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.BasicInfo
{
    public class Location : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 위치명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 비고
        /// </summary>
        public string? Note { get; private set; }

        /// <summary>
        /// 상위 위치
        /// </summary>
        public Location? UpperLocation { get; private set; }
        public long? UpperLocationId { get; private set; }

        /// <summary>
        /// 루트 위치 여부
        /// </summary>
        public bool IsRoot { get; private set; }

        private Location() { }
        public Location(Location? upperLocation, string name)
        {
            UpperLocation = upperLocation;
            UpperLocationId = upperLocation?.Id;
            IsRoot = upperLocation == null;
            SetName(name);
        }


        /// <summary>
        /// 구역명을 변경한다.
        /// </summary>
        /// <param name="name">구역명</param>
        /// <exception cref="EmptyNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyValueException(nameof(name));
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
