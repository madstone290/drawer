using Drawer.WebClient.Api;
using MudBlazor;

namespace Drawer.WebClient.Presenters
{
    /// <summary>
    /// 스낵바 출력기능을 제공하는 프레젠터
    /// </summary>
    public class SnackbarPresenter
    {
        private readonly ISnackbar _snackbar;

        private readonly ApiClient _apiClient;

        /// <summary>
        /// 에러 알림 노출 여부
        /// </summary>
        protected bool ShowErrorMessage { get; set; } = true;

        /// <summary>
        /// 저장 성공알림 노출 여부
        /// </summary>
        protected bool ShowSuccessMessage { get; set; } = true;

        /// <summary>
        /// 저장 성공 메시지
        /// </summary>
        protected string SuccessMessage { get; set; } = "저장되었습니다";


        public SnackbarPresenter(ApiClient apiClient, ISnackbar snackbar)
        {
            _apiClient = apiClient;
            _snackbar = snackbar;
        }

        /// <summary>
        /// 데이터를 불러온다.
        /// 오류 발생시 뷰에 오류메시지를 출력한다.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        protected async Task<ApiResponseMessage<TResponse>> LoadAsync<TResponse>(ApiRequestMessage<TResponse> apiRequest)
        {
            var apiResponse = await _apiClient.SendAsync(apiRequest);
            if (!apiResponse.IsSuccessful && ShowSuccessMessage)
            {
                _snackbar.Add(apiResponse.ErrorMessage);
            }
            return apiResponse;
        }

        /// <summary>
        /// 데이터를 저장한다.
        /// 저장이 성공할 경우 성공메시지를 출력한다.
        /// 실패할 경우 오류메시지를 출력한다.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        protected async Task<ApiResponseMessage<TResponse>> SaveAsync<TResponse>(ApiRequestMessage<TResponse> apiRequest)
        {
            var apiResponse = await _apiClient.SendAsync(apiRequest);
            if (!apiResponse.IsSuccessful && ShowErrorMessage)
            {
                _snackbar.Add(apiResponse.ErrorMessage, Severity.Error);
            }
            else if (apiResponse.IsSuccessful && ShowSuccessMessage)
            {
                _snackbar.Add(SuccessMessage, Severity.Success);
            }
            return apiResponse;
        }


    }
}

