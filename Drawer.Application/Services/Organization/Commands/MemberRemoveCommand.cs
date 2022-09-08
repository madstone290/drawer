using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Organization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    public record MemberRemoveCommand(long CompanyId, MemberCommandModel CompanyMember) : ICommand;

    public class MemberRemoveCommandHandler : ICommandHandler<MemberRemoveCommand>
    {
        private readonly ICompanyMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MemberRemoveCommandHandler(ICompanyMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(MemberRemoveCommand command, CancellationToken cancellationToken)
        {
            var memberDto = command.CompanyMember;
            var member = await _memberRepository.FindByUserIdAsync(memberDto.UserId)
                ?? throw new EntityNotFoundException("멤버를 찾을 수 없습니다", new { memberDto.UserId });

            if (member.IsOwner)
                throw new AppException("회사 소유자를 직접 삭제할 수 없습니다");

            if (member != null && command.CompanyId == member.CompanyId)
                _memberRepository.Remove(member);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
