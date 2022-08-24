using Drawer.Application.Config;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Application.Services.Inventory.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Inventory.Queries
{
    public record GetLayoutByIdQuery(long Id) : IQuery<LayoutQueryModel?>;

    public class GetLayoutQueryHandler : IQueryHandler<GetLayoutByIdQuery, LayoutQueryModel?>
    {
        private readonly ILayoutRepository _layoutRepository;

        public GetLayoutQueryHandler(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public async Task<LayoutQueryModel?> Handle(GetLayoutByIdQuery query, CancellationToken cancellationToken)
        {
            var layout = await _layoutRepository.QueryById(query.Id);

            return layout;
        }
    }
}
