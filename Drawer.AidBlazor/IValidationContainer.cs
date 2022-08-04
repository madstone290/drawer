using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.AidBlazor
{
    public interface IValidationContainer
    {
        void AddValidation(IValidation validation);

        void RemoveValidation(IValidation validation);
    }
}
