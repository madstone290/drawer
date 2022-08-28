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
    public record GetLayoutByLocationQuery(long LocationId) : IQuery<LayoutQueryModel?>;

    public class GetLayoutByLocationQueryHandler : IQueryHandler<GetLayoutByLocationQuery, LayoutQueryModel?>
    {
        private readonly ILayoutRepository _layoutRepository;

        public GetLayoutByLocationQueryHandler(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public async Task<LayoutQueryModel?> Handle(GetLayoutByLocationQuery query, CancellationToken cancellationToken)
        {
            var layout = await _layoutRepository.QueryByLocation(query.LocationId);

            return layout;
        }
    }
}


