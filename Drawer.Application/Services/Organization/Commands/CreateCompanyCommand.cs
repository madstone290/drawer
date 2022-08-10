using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    public record CreateCompanyCommand(string OwnerId, CompanyAddUpdateCommandModel Company) : ICommand<string>;

    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, string>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;

        public CreateCompanyCommandHandler(UserManager<IdentityUser> userManager, IOrganizationUnitOfWork organizationUnitOfWork)
        {
            _userManager = userManager;
            _organizationUnitOfWork = organizationUnitOfWork;
        }

        public async Task<string> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            // 회사를 생성하고 회사와 사용자간의 참조를 생성한다.
            var user = await _userManager.FindByIdAsync(command.OwnerId)
                ?? throw new InvalidUserIdException();

            if (await _organizationUnitOfWork.CompanyRepository.ExistByOwnerId(user.Id))
                throw new CompanyAlreadyExistException();

            var companyDto = command.Company;

            var company = new Company(user.Id, companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);
            await _organizationUnitOfWork.CompanyRepository.AddAsync(company);

            var companyMember = new CompanyMember(company.Id, user.Id, true);
            await _organizationUnitOfWork.CompanyMemberRepository.AddAsync(companyMember);

            await _organizationUnitOfWork.SaveChangesAsync();
            return company.Id;
        }
    }
}
