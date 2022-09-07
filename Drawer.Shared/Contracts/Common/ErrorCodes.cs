using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Shared.Contracts.Common
{
    public class ErrorCodes
    {
        public const string UnconfirmedEmail = "UnconfirmedEmail";

        /// <summary>
        /// 엔티티를 찾을 수 없다
        /// </summary>
        public const string ENTITY_NOT_FOUND = "ENTITY_NOT_FOUND";

        public const string FOREIGN_KEY_VIOLATION = "FOREIGN_KEY_VIOLATION";
    }
}
