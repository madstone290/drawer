using Drawer.Application.Config;
using Drawer.Application.Services.Organization.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Queries
{
    public record GetCompanyQuery(string Id) : IQuery<GetCompanyResult?>;

    public record GetCompanyResult(string Id, string OwnerId, string Name, string? PhoneNumber);

    public class GetCompanyQueryHandler : IQueryHandler<GetCompanyQuery, GetCompanyResult?>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<GetCompanyResult?> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id);
            if (company == null)
                return null;
            return new GetCompanyResult(company.Id, company.OwnerId, company.Name, company.PhoneNumber);
                
        }
    }
}
