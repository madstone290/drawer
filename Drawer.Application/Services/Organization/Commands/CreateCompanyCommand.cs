using Drawer.Application.Config;
using Drawer.Application.DomainServices;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Services;
using System.Runtime.CompilerServices;

namespace Drawer.Application.Services.Organization.Commands
{
    public record CreateCompanyCommand(string IdentityUserId, CompanyCommandModel Company) : ICommand<long>;

    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyJoinService _companyJoinService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyJoinRequestRepository _joinRequestRepository;
        private readonly ICompanyMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyCommandHandler(IUserRepository userRepository,
                                           ICompanyJoinService companyJoinService,
                                           ICompanyRepository companyRepository,
                                           ICompanyJoinRequestRepository joinRequestRepository,
                                           ICompanyMemberRepository memberRepository,
                                           IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _companyJoinService = companyJoinService;
            _companyRepository = companyRepository;
            _joinRequestRepository = joinRequestRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            // 회사를 생성하고 회사와 사용자간의 참조를 생성한다.
            var user = await _userRepository.FindByIdentityUserId(command.IdentityUserId)
                ?? throw new EntityNotFoundException("사용자를 찾을 수 없습니다", new { command.IdentityUserId });

            if (await _companyRepository.ExistByOwnerId(user.Id))
                throw new CompanyAlreadyExistException();

            var companyDto = command.Company;

            var company = new Company(user, companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);
            await _companyRepository.AddAsync(company);

            await _companyJoinService.Join(company, user, true);

            await _unitOfWork.CommitAsync();
            return company.Id;
        }
    }
}
