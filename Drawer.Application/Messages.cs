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
        public const string InvalidUserId = "유효하지 않은 사용자 아이디입니다";
        public const string InvalidLogin = "로그인정보가 유효하지 않습니다";
        public const string InvalidRefreshToken = "리프레시 토큰이 유효하지 않습니다";
        
        public const string NotExistingUser = "존재하지 않는 사용자입니다";
        public const string PasswordMitmatch = "비밀번호가 일치하지 않습니다";
        public const string CompanyAlreadyExist = "이미 등록된 회사가 존재합니다";


        public const string ConfirmationEmailSubject = "회원가입 이메일 확인";
        public const string ConfirmationEmailText = "안녕하세요 드로어입니다. 회원 가입을 환영합니다. 링크를 눌러 메일을 검증하고 회원 가입을 완료하세요.";
    }
}
