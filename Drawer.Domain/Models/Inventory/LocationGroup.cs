using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Inventory
{
    public class LocationGroup : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 그룹명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 비고
        /// </summary>
        public string? Note { get; private set; }

        /// <summary>
        /// 부모 그룹
        /// </summary>
        public LocationGroup? ParentGroup { get; private set; }
        public long? ParentGroupId { get; private set; }

        /// <summary>
        /// 계층 깊이. 루트가 0이며 1씩 증가한다.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// DB에 저장된 루트그룹ID.
        /// 루트그룹인 경우 DB저장 전까지 ID를 확인할 수 없기때문에 null이 저장된다.
        /// RootGroupId 속성을 통해 실제 루트그룹ID를 확인할 수 있다.
        /// </summary>
        public long? RootGroupIdDBValue { get; private set; }

        /// <summary>
        /// 루트 그룹
        /// </summary>
        public long RootGroupId => RootGroupIdDBValue ?? Id;

        /// <summary>
        /// 루트 그룹인가?
        /// </summary>
        public bool IsRoot => Depth == 0;

        private LocationGroup() { }
    
        public LocationGroup(string name, LocationGroup? parentGroup = null)
        {
            SetName(name);

            ParentGroup = parentGroup;
            ParentGroupId = parentGroup?.Id;
            Depth = parentGroup == null ? 0 : parentGroup.Depth + 1;
            RootGroupIdDBValue = parentGroup?.RootGroupId;
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
