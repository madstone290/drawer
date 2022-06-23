using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application
{
    public class Messages
    {
        public const string DuplicateEmail = "중복 이메일입니다";
        public const string UnconfirmedEmail = "인증되지 않은 이메일입니다";
        public const string InvalidEmail = "유효하지 않은 이메일입니다";
        public const string InvalidLogin = "로그인정보가 유효하지 않습니다";
        public const string InvalidRefreshToken = "리프레시 토큰이 유효하지 않습니다";

        public const string ConfirmEmailSubject = "회원가입 이메일 확인";
        public const string ClickLinkToVerify = "링크를 눌러 메일을 검증하세요";

        public const string NotExistingUser = "존재하지 않는 사용자입니다";

        public const string PasswordMitmatch = "비밀번호가 일치하지 않습니다";

    }
}
