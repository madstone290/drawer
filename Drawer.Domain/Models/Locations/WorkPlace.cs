using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Locations
{
    /// <summary>
    /// 사업장
    /// </summary>
    public class WorkPlace : CompanyResourceEntity<long>
    {
        /// <summary>
        /// 사업장명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 비고
        /// </summary>
        public string? Note { get; private set; } 

        public WorkPlace(string name)
        {
            SetName(name);
        }

        /// <summary>
        /// 사업장명을 변경한다. 
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="EmptyNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException("장소 이름이 비었습니다");
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
