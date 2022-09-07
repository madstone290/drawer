using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Models.UserInformation;
using Drawer.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Drawer.Shared.ApiRoutes;

namespace Drawer.Application.Services.Organization.Commands
{
    /// <summary>
    /// 가입요청을 처리한다
    /// </summary>
    /// <param name="JoinRequest"></param>
    public record JoinRequestHandleCommand(long JoinRequestId, JoinRequestHandleCommandModel JoinRequest) : ICommand;

    public class JoinRequestHandleCommandHandler : ICommandHandler<JoinRequestHandleCommand>
    {
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;
        private readonly ICompanyJoinService _companyJoinService;

        public JoinRequestHandleCommandHandler(IOrganizationUnitOfWork organizationUnitOfWork, ICompanyJoinService companyJoinService)
        {
            _organizationUnitOfWork = organizationUnitOfWork;
            _companyJoinService = companyJoinService;
        }

        public async Task<Unit> Handle(JoinRequestHandleCommand command, CancellationToken cancellationToken)
        {
            var requestDto = command.JoinRequest;

            var joinRequest = await _organizationUnitOfWork.JoinRequestRepository.FindByIdAsync(command.JoinRequestId)
                ?? throw new EntityNotFoundException("가입요청을 찾을 수 없습니다", new { command.JoinRequestId });

            joinRequest.Handle(requestDto.IsAccepted);

            if (!requestDto.IsAccepted)
                return Unit.Value;

            var result = await _companyJoinService.Join(joinRequest.Company, joinRequest.User, true);

            var companyMember = result.Item1;
            await _organizationUnitOfWork.MemberRepository.AddAsync(companyMember);

            var requestsToDelete = result.Item2;
            foreach (var request in requestsToDelete)
                _organizationUnitOfWork.JoinRequestRepository.Remove(request);

            await _organizationUnitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
