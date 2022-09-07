using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Organization.CommandModels
{
    /// <summary>
    /// 가입요청 처리 모델
    /// </summary>
    public class JoinRequestHandleCommandModel
    {
        /// <summary>
        /// 가입 승인 여부
        /// </summary>
        public bool IsAccepted { get; set; }
    }
}
