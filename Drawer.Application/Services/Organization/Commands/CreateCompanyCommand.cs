using Drawer.Application.Config;
using Drawer.Application.DomainServices;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using Drawer.Domain.Services;

namespace Drawer.Application.Services.Organization.Commands
{
    public record CreateCompanyCommand(string IdentityUserId, CompanyCommandModel Company) : ICommand<long>;

    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;
        private readonly ICompanyJoinService _companyJoinService;

        public CreateCompanyCommandHandler(IUserRepository userRepository, IOrganizationUnitOfWork organizationUnitOfWork, ICompanyJoinService companyJoinService)
        {
            _userRepository = userRepository;
            _organizationUnitOfWork = organizationUnitOfWork;
            _companyJoinService = companyJoinService;
        }

        public async Task<long> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            // 회사를 생성하고 회사와 사용자간의 참조를 생성한다.
            var user = await _userRepository.FindByIdentityUserId(command.IdentityUserId)
                ?? throw new EntityNotFoundException("사용자를 찾을 수 없습니다", new { command.IdentityUserId });

            if (await _organizationUnitOfWork.CompanyRepository.ExistByOwnerId(user.Id))
                throw new CompanyAlreadyExistException();

            var companyDto = command.Company;

            var company = new Company(user, companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);
            await _organizationUnitOfWork.CompanyRepository.AddAsync(company);

            var result = await _companyJoinService.Join(company, user, true);

            var companyMember = result.Item1;
            await _organizationUnitOfWork.MemberRepository.AddAsync(companyMember);

            var requestsToDelete = result.Item2;
            foreach (var request in requestsToDelete)
                _organizationUnitOfWork.JoinRequestRepository.Remove(request);

            await _organizationUnitOfWork.SaveChangesAsync();
            return company.Id;
        }
    }
}
