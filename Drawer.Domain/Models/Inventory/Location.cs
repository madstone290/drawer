using Drawer.Domain.Config;
using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
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
        /// 위치가 소속된 그룹
        /// </summary>
        public LocationGroup Group { get; private set; } = null!;
        public long GroupId { get; private set; }

        /// <summary>
        /// 루트 그룹 ID.
        /// </summary>
        public long RootGroupId { get; private set; }

        private Location() { }
        public Location(LocationGroup group, string name)
        {
            SetGroup(group);
            SetName(name);
        }

        private void SetGroup(LocationGroup group)
        {
            if (group == null)
                throw new DomainException("그룹이 null입니다");

            Group = group;
            GroupId = group.Id;
            RootGroupId = group.RootGroupId;
        }

        /// <summary>
        /// 위치명을 변경한다.
        /// </summary>
        /// <param name="name">위치명</param>
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
