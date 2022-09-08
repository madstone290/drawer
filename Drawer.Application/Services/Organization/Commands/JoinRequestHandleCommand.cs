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
        private readonly ICompanyJoinService _companyJoinService;
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JoinRequestHandleCommandHandler(ICompanyJoinService companyJoinService,
                                               ICompanyJoinRequestRepository joinRequestRepository,
                                               IUnitOfWork unitOfWork)
        {
            _companyJoinService = companyJoinService;
            _joinRequestRepository = joinRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(JoinRequestHandleCommand command, CancellationToken cancellationToken)
        {
            var requestDto = command.JoinRequest;

            var joinRequest = await _joinRequestRepository.FindByIdAsync(command.JoinRequestId)
                ?? throw new EntityNotFoundException("가입요청을 찾을 수 없습니다", new { command.JoinRequestId });

            joinRequest.Handle(requestDto.IsAccepted);

            if (!requestDto.IsAccepted)
                return Unit.Value;

            await _companyJoinService.Join(joinRequest.Company, joinRequest.User, false);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
