using Drawer.Application.Config;
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
    public record CompanyMemberRemoveCommand(long CompanyId, CompanyMemberCommandModel CompanyMember) : ICommand;

    public class CompanyMemberRemoveCommandHandler : ICommandHandler<CompanyMemberRemoveCommand>
    {
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;

        public CompanyMemberRemoveCommandHandler(IOrganizationUnitOfWork organizationUnitOfWork)
        {
            _organizationUnitOfWork = organizationUnitOfWork;
        }

        public async Task<Unit> Handle(CompanyMemberRemoveCommand command, CancellationToken cancellationToken)
        {
            var memberDto = command.CompanyMember;
            var member = await _organizationUnitOfWork.CompanyMemberRepository.FindByUserIdAsync(memberDto.UserId);
            if(member != null && command.CompanyId == member.CompanyId)
                _organizationUnitOfWork.CompanyMemberRepository.Remove(member);

            await _organizationUnitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
