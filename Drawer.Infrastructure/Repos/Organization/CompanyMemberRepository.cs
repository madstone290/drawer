using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Organization
{
    public class CompanyMemberRepository : Repository<CompanyMember>, ICompanyMemberRepository
    {
        public CompanyMemberRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<CompanyMember>> FindByCompanyIdAsync(string companyId)
        {
            return await _dbContext.CompanyMembers.Where(x => x.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<CompanyMember?> FindByUserIdAsync(string userId)
        {
            return await _dbContext.CompanyMembers
                .FirstOrDefaultAsync(x => x.UserId == userId);
         
        }
    }
}
