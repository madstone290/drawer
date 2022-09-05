using Drawer.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Services.Organization
{
    public class CompanyIdProvider : ICompanyIdProvider
    {
        private readonly long? _companyId;

        public CompanyIdProvider(IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext == null)
            {
                _companyId = null;
            }
            else
            {
                
                _companyId = Convert.ToInt64(accessor.HttpContext.User
                    .Claims.FirstOrDefault(x => x.Type == DrawerClaimTypes.CompanyId)
                    ?.Value);
            }
        }

        public long? GetCompanyId()
        {
            return _companyId;
        }
    }
}
