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
        public Location? ParentGroup { get; private set; }
        public long? ParentGroupId { get; private set; }

        /// <summary>
        /// 루트 그룹 ID.
        /// </summary>
        public long? RootGroupId { get; private set; }

        /// <summary>
        /// 실제 루트그룹ID.
        /// 루트그룹의 RootGroupId 속성 값은 null이다.
        /// </summary>
        public long ActualRootGroupId => RootGroupId.HasValue ? RootGroupId.Value : Id;


        /// <summary>
        /// 계층 레벨. 0부터시작해서 1씩 증가한다.
        /// </summary>
        public int HierarchyLevel { get; private set; }

        /// <summary>
        /// 위치 그룹인가?
        /// 그룹은 다른 위치를 포함한다.
        /// 그룹에는 아이템을 직접적으로 보관할 수 없다. 
        /// </summary>
        public bool IsGroup { get; private set; }

        /// <summary>
        /// 루트 그룹인가?
        /// </summary>
        public bool IsRootGroup => IsGroup && HierarchyLevel == 0;

        private Location() { }
        public Location(Location? parentGroup, string name, bool isGroup)
        {
            if (parentGroup == null && isGroup == false)
                throw new DomainException("위치 등록에는 그룹이 필요합니다");

            SetParentGroup(parentGroup);
            SetName(name);
            IsGroup = isGroup;
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

        public void SetParentGroup(Location? parentGroup)
        {
            if (parentGroup != null && !parentGroup.IsGroup)
                throw new DomainException($"{parentGroup.Name}은 위치 그룹이 아닙니다");

            ParentGroup = parentGroup;
            ParentGroupId = parentGroup?.Id;
            HierarchyLevel = parentGroup == null ? 0 : parentGroup.HierarchyLevel + 1;
            RootGroupId = parentGroup?.RootGroupId;
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
