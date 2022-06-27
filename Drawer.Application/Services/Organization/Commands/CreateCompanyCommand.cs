using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Authentication;
using Drawer.Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Commands
{
    /// <summary>
    /// 회사를 생성한다
    /// </summary>
    /// <param name="UserId">회사를 생성할 사용자의 Email</param>
    /// <param name="Name"></param>
    /// <param name="PhoneNumber"></param>
    public record CreateCompanyCommand(string UserId, string Name, string? PhoneNumber) : ICommand<CreateCompanyResult>;

    public record CreateCompanyResult(string Id, string OwnerId, string Name, string? PhoneNumber);

    public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, CreateCompanyResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrganizationUnitOfWork _organizationUnitOfWork;

        public CreateCompanyCommandHandler(UserManager<User> userManager, IOrganizationUnitOfWork organizationUnitOfWork)
        {
            _userManager = userManager;
            _organizationUnitOfWork = organizationUnitOfWork;
        }

        public async Task<CreateCompanyResult> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            // 회사를 생성하고 회사와 사용자간의 참조를 생성한다.
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new InvalidUserIdException();

            if (await _organizationUnitOfWork.CompanyRepository.ExistByOwnerId(user.Id))
                throw new CompanyAlreadyExistException();

            var company = new Company(user.Id, request.Name);
            company.SetPhoneNumber(request.PhoneNumber);
            await _organizationUnitOfWork.CompanyRepository.AddAsync(company);

            var companyMember = new CompanyMember(company.Id, user.Id);
            await _organizationUnitOfWork.CompanyMemberRepository.AddAsync(companyMember);

            await _organizationUnitOfWork.SaveChangesAsync();
            return new CreateCompanyResult(company.Id, company.OwnerId, company.Name, company.PhoneNumber);
        }
    }
}
