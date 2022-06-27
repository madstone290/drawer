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
    public class WorkPlace : CompanyEntity<long>
    {
        /// <summary>
        /// 사업장명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 사업장 설명
        /// </summary>
        public string? Description { get; private set; } 

        public WorkPlace(string companyId, string name) : base(companyId)
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
                throw new EmptyNameException("사업장명이 비었습니다");
            Name = name.Trim();
        }

        /// <summary>
        /// 사업장 설명을 변경한다.
        /// </summary>
        /// <param name="description"></param>
        public void SetDescription(string description)
        {
            Description = description?.Trim();
        }

    }
}
