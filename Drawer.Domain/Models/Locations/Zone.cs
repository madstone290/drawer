using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 장소 내 구역
    /// </summary>
    public class Zone : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 구역명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 구역이 포함된 장소
        /// </summary>
        public WorkPlace WorkPlace { get; private set; } = null!;

        public long WorkPlaceId { get; private set; }

        /// <summary>
        /// 비고
        /// </summary>
        public string? Note { get; private set; }

        private Zone() { }
        public Zone(WorkPlace workPlace, string name)
        {
            if (workPlace == null)
                throw new EntityNullException<WorkPlace>(nameof(workPlace));
            WorkPlace = workPlace;
            WorkPlaceId = workPlace.Id;
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
                throw new EmptyNameException("구역 이름이 비었습니다");
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
