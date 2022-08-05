namespace Drawer.Web.Api
{
    public interface IApiResponse
    {
        /// <summary>
        /// 요청 성공여부
        /// </summary>
        bool IsSuccessful { get; set; }

        /// <summary>
        /// 요청에 대한 권한 여부
        /// </summary>
        bool IsUnauthorized { get; set; }

        /// <summary>
        /// 에러 식별 코드
        /// </summary>
        string? ErrorCode { get; set; }

        /// <summary>
        /// 에러 메시지
        /// </summary>
        string? ErrorMessage { get; set; }

    }

    public class ApiResponse<TData> : IApiResponse
    {
        public bool IsSuccessful { get; set; }

        public bool IsUnauthorized { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 페이로드 데이터
        /// </summary>
        public TData Data { get; set; } = default!;

        /// <summary>
        /// 성공 응답을 받았다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponse<TData> Success(TData data) =>
            new() { Data = data, IsSuccessful = true };

        /// <summary>
        /// 실패 응답을 받았다.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static ApiResponse<TData> Fail(string errorMessage, string? errorCode = null) => 
            new() { ErrorCode = errorCode, ErrorMessage = errorMessage, IsSuccessful = false };

        /// <summary>
        /// 권한이 없어 요청을 실행할 수 없다.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static ApiResponse<TData> Unauthorized(string error) => 
            new() { ErrorMessage = error, IsSuccessful = false, IsUnauthorized = true };

    }

    public class ApiResponse : ApiResponse<Unit>
    {
    }

}
