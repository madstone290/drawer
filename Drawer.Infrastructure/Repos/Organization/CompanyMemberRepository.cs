﻿using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Infrastructure.Data;
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
    }
}