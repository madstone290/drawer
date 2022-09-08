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
    public class CompanyJoinRequestRepository : Repository<CompanyJoinRequest, long>, ICompanyJoinRequestRepository
    {
        public CompanyJoinRequestRepository(DrawerDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<CompanyJoinRequest?> FindByIdAsync(long id)
        {
            return await _dbContext.CompanyJoinRequests
                .Include(x => x.Company)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<bool> ExistUnhandledRequestByCompanyIdAndUserId(long companyId, long userId)
        {
            return await _dbContext.CompanyJoinRequests
                .AnyAsync(x => x.CompanyId == companyId && x.UserId == userId && x.IsHandled == false);
        }

        public async Task<CompanyJoinRequest?> GetUnhandledRequestByCompanyIdAndUserId(long companyId, long userId)
        {
            return await _dbContext.CompanyJoinRequests
                   .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UserId == userId && x.IsHandled == false);
        }

        public async Task<List<CompanyJoinRequest>> FindUnhandledRequestByUserId(long userId)
        {
            return await _dbContext.CompanyJoinRequests
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<CompanyJoinRequestQueryModel>> QueryByCompanyId(long companyId)
        {
            return await _dbContext.CompanyJoinRequests
                .Include(x=> x.User)
                .Where(x => x.CompanyId == companyId)
                .Select(x => new CompanyJoinRequestQueryModel()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    IsHandled = x.IsHandled,
                    RequestTimeUtc = x.RequestTimeUtc,
                    UserEmail = x.User.Email,
                    UserName = x.User.Name
                })
                .ToListAsync();
        }

  
    }
}
