using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.AidBlazor
{
    /// <summary>
    /// 유효성 검사를 제공하는 컴포넌트를 가진다.
    /// </summary>
    public interface IValidationContainer
    {
        void AddValidation(IValidation validation);

        void RemoveValidation(IValidation validation);
    }
}
