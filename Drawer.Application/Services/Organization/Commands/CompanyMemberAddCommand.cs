using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    public record CompanyMemberAddCommand(long CompanyId, CompanyMemberCommandModel CompanyMember) : ICommand;

    public class CompanyMemberAddCommandHandler : ICommandHandler<CompanyMemberAddCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyMemberRepository _memberRepository;
        private readonly IUserRepository _userRepository;

        public CompanyMemberAddCommandHandler(ICompanyRepository companyRepository, ICompanyMemberRepository memberRepository, IUserRepository userRepository)
        {
            _companyRepository = companyRepository;
            _memberRepository = memberRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(CompanyMemberAddCommand command, CancellationToken cancellationToken)
        {
            var memberDto = command.CompanyMember;

            bool memberExist = await _memberRepository.ExistByUserId(memberDto.UserId);
            if (memberExist)
                throw new AppException("이미 회사에 가입한 사용자 입니다");

            var company = await _companyRepository.FindByIdAsync(command.CompanyId)
                ?? throw new AppException("회사를 찾을 수 없습니다");

            var user = await _userRepository.FindByIdAsync(memberDto.UserId)
                 ?? throw new AppException("사용자를 찾을 수 없습니다");

            var companyMember = new CompanyMember(company, user, false);

            await _memberRepository.AddAsync(companyMember);
            await _memberRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
