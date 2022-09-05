using Drawer.Application.Services.Organization.QueryModels;
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
    public class CompanyRepository : Repository<Company, long>, ICompanyRepository
    {
        public CompanyRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistByOwnerId(long ownerId)
        {
            return await _dbContext.Companies.AnyAsync(x => x.OwnerId == ownerId);
        }

        public async Task<CompanyQueryModel?> QueryById(long id)
        {
            return await _dbContext.Companies
                .Where(x => x.Id == id)
                .Select(x => new CompanyQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerId = x.OwnerId,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefaultAsync();
                
        }
    }
}
