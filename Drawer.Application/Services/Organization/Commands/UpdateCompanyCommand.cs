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
    public record UpdateCompanyCommand(string Id, string Name, string? PhoneNumber) : ICommand<UpdateCompanyResult>;

    public record UpdateCompanyResult;

    public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand, UpdateCompanyResult>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<UpdateCompanyResult> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id);
            if (company == null)
                throw new EntityNotFoundException<Company>(request.Id);

            company.SetName(request.Name);
            company.SetPhoneNumber(request.PhoneNumber);
            await _companyRepository.SaveChangesAsync();

            return new UpdateCompanyResult();
        }
    }
}
