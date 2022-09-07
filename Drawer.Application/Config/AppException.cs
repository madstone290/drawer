using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Config
{
    public class AppException : Exception
    {
        /// <summary>
        /// 에러 상세 데이터 객체
        /// </summary>
        public object? Tag { get; set; }

        /// <summary>
        /// 에러 식별을 위한 코드
        /// </summary>
        public string? Code { get; set; }

        public AppException(string message, object? tag = null, string? code = null) : base(message)
        {
            Tag = tag;
            Code = code;
        }
    }
}
