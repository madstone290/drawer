using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Domain.Models
{
    /// <summary>
    /// 회사에 포함되는 엔티티.
    /// 회사에 속한 구성원들만 접근가능하다.
    /// </summary>
    public abstract class CompanyResourceEntity<TId> : AuditableEntity<TId>, ICompanyResource
    {
        private string _companyId = string.Empty;
        /// <summary>
        /// 회사 ID
        /// </summary>
        public string CompanyId 
        {
            get => _companyId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("유효하지 않은 회사Id입니다");
                if (!string.IsNullOrWhiteSpace(_companyId))
                    throw new Exception("회사Id가 이미 설정되었습니다");
                _companyId = value;
            } 
        } 



     
    }
}
