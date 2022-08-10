using Drawer.Application.Config;
using Drawer.Application.Services.Organization.QueryModels;
using Drawer.Application.Services.Organization.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.Queries
{
    public record GetCompanyByIdQuery(string Id) : IQuery<CompanyQueryModel?>;

    public class GetCompanyQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyQueryModel?>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyQueryModel?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.QueryById(request.Id);
            return company;
                
        }
    }
}
