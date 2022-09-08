using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Shared.Contracts.Common
{
    /// <summary>
    /// API에서 발생하는 에러 코드.
    /// 에러코드에 맞춰 적절한 처리를 해준다.
    /// </summary>
    public class ErrorCodes
    {
        public const string UNCONFIRMED_EMAIL = "UNCONFIRMED_EMAIL";

        /// <summary>
        /// 엔티티를 찾을 수 없다
        /// </summary>
        public const string ENTITY_NOT_FOUND = "ENTITY_NOT_FOUND";

        public const string FOREIGN_KEY_VIOLATION = "FOREIGN_KEY_VIOLATION";
    }
}
