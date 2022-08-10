using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Shared.Contracts.Common
{
    public record ErrorResponse(string Message, string? Code = null);
}
