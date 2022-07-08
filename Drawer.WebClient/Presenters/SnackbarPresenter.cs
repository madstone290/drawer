using Drawer.WebClient.Api;
using MudBlazor;

namespace Drawer.WebClient.Presenters
{
    /// <summary>
    /// 스낵바 출력기능을 제공하는 프레젠터
    /// </summary>
    public class SnackbarPresenter : IPresenter
    {
        protected readonly ISnackbar _snackbar;

        /// 저장 성공 메시지
        /// </summary>
        protected string SuccessMessage { get; set; } = "저장하였습니다";

        public SnackbarPresenter(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        /// <summary>
        /// Api 응답이 실패한 경우 메시지를 출력한다.
        /// </summary>
        protected bool CheckFail<T>(ApiResponse<T> response)
        {
            if (!response.IsSuccessful)
            {
                _snackbar.Add(response.ErrorMessage, Severity.Error);
            }
            return response.IsSuccessful;
        }

        /// <summary>
        /// Api 응답이 성공하거나 실패한 경우 메시지를 출력한다.
        /// </summary>
        protected bool CheckSuccessFail<T>(ApiResponse<T> response, string? successMessage = null)
        {
            if (response.IsSuccessful)
            {
                _snackbar.Add(successMessage ?? SuccessMessage, Severity.Success);
            }

            if (!response.IsSuccessful)
            {
                _snackbar.Add(response.ErrorMessage, Severity.Error);
            }
            return response.IsSuccessful;
        }

    }
}

