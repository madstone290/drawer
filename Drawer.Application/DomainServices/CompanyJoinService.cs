using Drawer.Application.Config;
using Drawer.Application.Services.Organization;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using Drawer.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.DomainServices
{
    public class CompanyJoinService : ICompanyJoinService
    {
        private readonly ICompanyMemberRepository _memberRepository;
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;

        public CompanyJoinService(ICompanyMemberRepository memberRepository, ICompanyJoinRequestRepository joinRequestRepository)
        {
            _memberRepository = memberRepository;
            _joinRequestRepository = joinRequestRepository;
        }

        public async Task Join(Company company, User user, bool isOwner)
        {
            bool memberExist = await _memberRepository.ExistByUserId(user.Id);
            if (memberExist)
                throw new AppException("이미 회사에 가입한 사용자 입니다", new { UserId = user.Id });

            var companyMember = new CompanyMember(company, user, isOwner);
            await _memberRepository.AddAsync(companyMember);

            var oldRequests = await _joinRequestRepository.FindUnhandledRequestByUserId(user.Id);
            var requestsToDelete = oldRequests.Where(x => x.CompanyId != company.Id);
            foreach (var request in requestsToDelete)
                _joinRequestRepository.Remove(request);
        }

    }
}
