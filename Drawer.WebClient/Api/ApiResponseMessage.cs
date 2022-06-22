namespace Drawer.WebClient.Api
{
    public class ApiResponseMessage<TData> 
    {
        /// <summary>
        /// 요청 성공여부
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// 로그인 리디렉트 여부
        /// </summary>
        public bool NeedToLogin { get; set; }

        /// <summary>
        /// 에러메시지
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// 페이로드 데이터
        /// </summary>
        public TData Data { get; set; } = default!;

        public ApiResponseMessage()
        {
        }

        /// <summary>
        /// 성공 응답을 받았다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponseMessage<TData> Success(TData data) => new() { Data = data, IsSuccessful = true };

        /// <summary>
        /// 실패 응답을 받았다.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ApiResponseMessage<TData> Fail(string error) => new() { Error = error, IsSuccessful = false };

        /// <summary>
        /// 권한이 없어 요청을 실행할 수 없다.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ApiResponseMessage<TData> Unauthorized(string error) => new() { Error = error, IsSuccessful = false, NeedToLogin = true };

    }

    public class ApiResponseMessage : ApiResponseMessage<Unit>
    {
    }

}
