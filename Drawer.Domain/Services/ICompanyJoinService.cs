using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Services
{
    public interface ICompanyJoinService
    {
        /// <summary>
        /// 회사에 가입한다
        /// </summary>
        /// <param name="company"></param>
        /// <param name="user"></param>
        /// <param name="isOwner"></param>
        /// <returns>생성한 멤버, 삭제할 가입요청내역</returns>
        Task Join(Company company, User user, bool isOwner);
    }

    
}
