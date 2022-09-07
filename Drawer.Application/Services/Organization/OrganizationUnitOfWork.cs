using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization
{
    public interface IOrganizationUnitOfWork : IUnitOfWork
    {
        public ICompanyRepository CompanyRepository { get; }
        public ICompanyMemberRepository MemberRepository { get; }
        public ICompanyJoinRequestRepository JoinRequestRepository { get; }
    }
}
