using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.UserInformation.Repos;
using Drawer.Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    public record CreateCompanyCommand(string IdentityUserId, CompanyAddUpdateCommandModel Company) : ICommand<long>;

    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;

        public CreateCompanyCommandHandler(IUserRepository userRepository, IOrganizationUnitOfWork organizationUnitOfWork)
        {
            _userRepository = userRepository;
            _organizationUnitOfWork = organizationUnitOfWork;
        }

        public async Task<long> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            // 회사를 생성하고 회사와 사용자간의 참조를 생성한다.
            var user = await _userRepository.FindByIdentityUserId(command.IdentityUserId)
                ?? throw new InvalidUserIdException();

            if (await _organizationUnitOfWork.CompanyRepository.ExistByOwnerId(user.Id))
                throw new CompanyAlreadyExistException();

            var companyDto = command.Company;

            var company = new Company(user, companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);
            await _organizationUnitOfWork.CompanyRepository.AddAsync(company);

            var companyMember = new CompanyMember(company, user, true);
            await _organizationUnitOfWork.CompanyMemberRepository.AddAsync(companyMember);

            await _organizationUnitOfWork.SaveChangesAsync();
            return company.Id;
        }
    }
}
