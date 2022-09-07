using Drawer.Web.Authentication;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Organization
{
    public partial class RegisterCompany
    {
        public enum RegisterType
        {
            /// <summary>
            /// 미선택
            /// </summary>
            None,
            
            /// <summary>
            /// 회사 생성
            /// </summary>
            CreateCompany,

            /// <summary>
            /// 가입 요청
            /// </summary>
            JoinRequest
        }

        /// <summary>
        /// 회사 등록방법 선택
        /// </summary>
        private RegisterType _selectedRegisterType;

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IAuthenticationManager AuthenticationManager { get; set; } = null!;
        [Inject] public ITokenManager TokenManager { get; set; } = null!;

        void CreateCompany_Click()
        {
            _selectedRegisterType = RegisterType.CreateCompany;
        }

        void Request_Click()
        {
            _selectedRegisterType = RegisterType.JoinRequest;
        }
    }
}
