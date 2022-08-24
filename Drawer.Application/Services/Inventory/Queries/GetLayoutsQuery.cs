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
    public record GetLayoutsQuery : IQuery<List<LayoutQueryModel>>;

    public class GetLayoutsQueryHandler : IQueryHandler<GetLayoutsQuery, List<LayoutQueryModel>>
    {
        private readonly ILayoutRepository _layoutRepository;

        public GetLayoutsQueryHandler(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public async Task<List<LayoutQueryModel>> Handle(GetLayoutsQuery query, CancellationToken cancellationToken)
        {
            var layouts = await _layoutRepository.QueryAll();

            return layouts;
        }
    }
}
