using Drawer.Application.Config;
using Drawer.Application.Exceptions;
using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Application.Services.Organization.Repos;
using Drawer.Domain.Models.Authentication;
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
    public record UpdateCompanyCommand(long Id, CompanyAddUpdateCommandModel Company) : ICommand;

    public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
        {
            var companyDto = command.Company;

            var company = await _companyRepository.FindByIdAsync(command.Id);
            if (company == null)
                throw new EntityNotFoundException<Company>(command.Id);

            company.SetName(companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);

            await _companyRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
