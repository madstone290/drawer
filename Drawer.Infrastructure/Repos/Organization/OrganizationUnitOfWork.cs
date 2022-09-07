using Drawer.Application.Services.Organization;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Repos.Organization
{
    public class OrganizationUnitOfWork : IOrganizationUnitOfWork
    {
        private readonly DrawerDbContext _dbContext;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyMemberRepository _companyMemberRepository;
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;

        public OrganizationUnitOfWork(
            DrawerDbContext dbContext,
            ICompanyRepository companyRepository,
            ICompanyMemberRepository companyMemberRepository,
            ICompanyJoinRequestRepository joinRequestRepository)
        {
            _dbContext = dbContext;
            _companyRepository = companyRepository;
            _companyMemberRepository = companyMemberRepository;
            _joinRequestRepository = joinRequestRepository;
        }

        public ICompanyRepository CompanyRepository => _companyRepository;

        public ICompanyMemberRepository MemberRepository => _companyMemberRepository;

        public ICompanyJoinRequestRepository JoinRequestRepository => _joinRequestRepository;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
