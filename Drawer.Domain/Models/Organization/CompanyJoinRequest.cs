using Drawer.Domain.Config;
using Drawer.Domain.Models.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models.Organization
{
    /// <summary>
    /// 회사 가입요청
    /// </summary>
    public class CompanyJoinRequest :  Entity<long>
    {
        /// <summary>
        /// 사용자가 가입할 회사
        /// </summary>
        public Company Company { get; private set; } = null!;
        public long CompanyId { get; private set; }

        /// <summary>
        /// 회사에 가입할 사용자
        /// </summary>
        public User User { get; private set; } = null!;
        public long UserId { get; private set; } 

        /// <summary>
        /// 가입 요청시간
        /// </summary>
        public DateTime RequestTimeUtc { get; private set; }

        /// <summary>
        /// 요청 처리 여부
        /// </summary>
        public bool IsHandled { get; private set; }

        /// <summary>
        /// 가입 승인 여부
        /// </summary>
        public bool IsAccepted { get; private set; }

        private CompanyJoinRequest() { }
        public CompanyJoinRequest(Company company, User user, DateTime requestTimeUtc)
        {
            Company = company;
            User = user;
            RequestTimeUtc = requestTimeUtc;
        }

        /// <summary>
        /// 요청이 처리됐음을 기록한다
        /// </summary>
        public void Handle(bool isAccpeted)
        {
            if (IsHandled)
                throw new DomainException("이미 처리된 요청입니다");

            IsHandled = true;
            IsAccepted = isAccpeted;
        }
    }
}
