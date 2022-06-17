using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication
{
    public interface IEmailSender
    {
        /// <summary>
        /// 이메일을 전송한다
        /// </summary>
        /// <param name="emailTo">수신 이메일</param>
        /// <param name="subject">이메일 제목</param>
        /// <param name="text">이메일 본문</param>
        Task SendEmailAsync(string emailTo, string subject, string text, bool isHtml);
    }
}
