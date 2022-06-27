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
        /// <summary>
        /// 조직 ID
        /// </summary>
        public string CompanyId { get; set; } = default!;

        protected CompanyResourceEntity(string companyId)
        {
            CompanyId = companyId;
        }
    }
}
