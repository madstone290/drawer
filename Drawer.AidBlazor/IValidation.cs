using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.AidBlazor
{
    public interface IValidation
    {
        Task<string?> ValidateAsync();
    }
}
