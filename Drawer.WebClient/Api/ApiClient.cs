using Drawer.WebClient.Token;
using Drawer.WebClient.Utils;

namespace Drawer.WebClient.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenManager _tokenManager;

        public ApiClient(HttpClient httpClient, ITokenManager tokenManager)
        {
            _httpClient = httpClient;
            _tokenManager = tokenManager;
        }

        public async Task<ApiResponseMessage<TResponseData>> SendAsync<TResponseData>(ApiRequestMessage<TResponseData> apiRequest)
        {
            var tokenResult = await _tokenManager.GetAccessTokenAsync();
            if(tokenResult.IsSuccessful == false) 
            {
                return ApiResponseMessage<TResponseData>.Unauthorized("액세스 토큰을 찾을 수 없습니다. 로그인이 필요합니다");
            }

            // add authentication
            var requestMessage = apiRequest.Request;
            requestMessage.SetBearerToken(tokenResult.AccessToken!);

            var responseMessage = await _httpClient.SendAsync(requestMessage);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // 액세스 토큰이 만료되었을 경우 권한없음 응답이 발생한다.
                // 권한이 없을 경우 토큰 갱신을 1회 실행한다.
                tokenResult = await _tokenManager.RefreshAccessTokenAsync();
                if (tokenResult.IsSuccessful == false)
                {
                    return ApiResponseMessage<TResponseData>.Unauthorized("토큰 갱신에 실패했습니다. 로그인이 필요합니다");
                }

                var cloneRequestMessage = await requestMessage.CloneAsync();
                cloneRequestMessage.SetBearerToken(tokenResult.AccessToken!);

                responseMessage = await _httpClient.SendAsync(cloneRequestMessage);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ApiResponseMessage<TResponseData>.Unauthorized("액세스 토큰이 유효하지 않습니다. 로그인이 필요합니다");
                }
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResult = await responseMessage.Content.ReadNullableJsonAsync<TResponseData>();
                if(jsonResult.IsSuccessful)
                    return ApiResponseMessage<TResponseData>.Success(jsonResult.Data!);
                else
                    return ApiResponseMessage<TResponseData>.Fail("성공응답의 Json변환에 실패하였습니다");
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var jsonResult = await responseMessage.Content.ReadNullableJsonAsync<ErrorMessage>();
                if (jsonResult.IsSuccessful)
                    return ApiResponseMessage<TResponseData>.Fail(jsonResult.Data!.Message);
                else
                    return ApiResponseMessage<TResponseData>.Fail("실패응답의 Json변환에 실패하였습니다");
            }
            else if(responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return ApiResponseMessage<TResponseData>.Fail("서버에서 오류가 발생하였습니다");
            }
            else
            {
                return ApiResponseMessage<TResponseData>.Fail(responseMessage.StatusCode.ToString());
            }

        }


    }
}
