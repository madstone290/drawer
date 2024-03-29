﻿using Drawer.Application.Services.Organization.QueryModels;
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

        public async Task<bool> ExistByUserId(long userId)
        {
            return await _dbContext.CompanyMembers.AnyAsync(x => x.UserId == userId);
        }

        public async Task<CompanyMember?> FindByUserIdAsync(long userId)
        {
            return await _dbContext.CompanyMembers
                .FirstOrDefaultAsync(x => x.UserId == userId);
         
        }

        public async Task<List<CompanyMemberQueryModel>> QueryByCompanyId(long companyId)
        {
            return await _dbContext.CompanyMembers.Where(x => x.CompanyId == companyId)
                .Include(x=> x.User)
                .Select(x=> new CompanyMemberQueryModel() 
                { 
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    UserEmail = x.User.Email,
                    UserName = x.User.Name
                })
                .ToListAsync();
        }
    }
}
