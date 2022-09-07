using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
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
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;

        public MemberRemoveCommandHandler(IOrganizationUnitOfWork organizationUnitOfWork)
        {
            _organizationUnitOfWork = organizationUnitOfWork;
        }

        public async Task<Unit> Handle(MemberRemoveCommand command, CancellationToken cancellationToken)
        {
            var memberDto = command.CompanyMember;
            var member = await _organizationUnitOfWork.MemberRepository.FindByUserIdAsync(memberDto.UserId)
                ?? throw new EntityNotFoundException("멤버를 찾을 수 없습니다", new { memberDto.UserId });

            if (member.IsOwner)
                throw new AppException("회사 소유자를 직접 삭제할 수 없습니다");

            if (member != null && command.CompanyId == member.CompanyId)
                _organizationUnitOfWork.MemberRepository.Remove(member);

            await _organizationUnitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
