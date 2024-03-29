﻿using Drawer.Application.Config;
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
    public record UpdateCompanyCommand(long Id, CompanyCommandModel Company) : ICommand;

    public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
        {
            var companyDto = command.Company;

            var company = await _companyRepository.FindByIdAsync(command.Id);
            if (company == null)
                throw new EntityNotFoundException<Company>(command.Id);

            company.SetName(companyDto.Name);
            company.SetPhoneNumber(companyDto.PhoneNumber);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
    }
}
