﻿using Drawer.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Organization
{
    public class Company : AuditableEntity<string>
    {
        /// <summary>
        /// 회사명
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// 회사를 생성한 사용자의 Id
        /// </summary>
        public string OwnerId { get; private set; }

        /// <summary>
        /// 회사번호
        /// </summary>
        public string? PhoneNumber { get; private set; }

        /// <summary>
        /// 회사를 생성한다
        /// </summary>
        /// <param name="ownerId">회사를 소유한 사용자의 Id</param>
        /// <param name="name">회사명</param>
        public Company(string ownerId, string name) 
        {
            OwnerId = ownerId;
            Id = Guid.NewGuid().ToString();
            SetName(name);
        }

        /// <summary>
        /// 회사명을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
             if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException();
            Name = name.Trim();
        }

        /// <summary>
        /// 회사 전화번호를 설정한다.
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void SetPhoneNumber(string? phoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
        }
    }
}
