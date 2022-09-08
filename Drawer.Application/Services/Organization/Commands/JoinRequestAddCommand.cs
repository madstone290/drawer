using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    public record JoinRequestAddCommand(long UserId, JoinRequestAddCommandModel Request) : ICommand<long>;

    public class JoinRequestAddCommandHandler : ICommandHandler<JoinRequestAddCommand, long>
    {
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JoinRequestAddCommandHandler(ICompanyJoinRequestRepository joinRequestRepository, ICompanyRepository companyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _joinRequestRepository = joinRequestRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(JoinRequestAddCommand command, CancellationToken cancellationToken)
        {
            var requestDto = command.Request;

            var company = await _companyRepository.FindByOwnerEmail(requestDto.OwnerEmail)
                 ?? throw new EntityNotFoundException("회사를 찾을 수 없습니다", new { OwnerEmail = requestDto.OwnerEmail });

            var user = await _userRepository.FindByIdAsync(command.UserId)
               ?? throw new EntityNotFoundException("사용자를 찾을 수 없습니다", new { Id = command.UserId });

            var unhandledRequest = await _joinRequestRepository.GetUnhandledRequestByCompanyIdAndUserId(company.Id, user.Id);

            if (unhandledRequest != null)
                return unhandledRequest.Id;

            var request = new CompanyJoinRequest(company, user, DateTime.UtcNow);
            await _joinRequestRepository.AddAsync(request);
            await _unitOfWork.CommitAsync();

            return request.Id;
        }
    }
}
