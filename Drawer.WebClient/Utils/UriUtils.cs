namespace Drawer.WebClient.Utils
{
    public static class UriUtils
    {
        /// <summary>
        /// 절대경로 Uri를 생성한다.
        /// </summary>
        /// <param name="request">현재 Http요청</param>
        /// <param name="relativePath">상대경로</param>
        /// <returns></returns>
        public static string GetAbsoluteUri(HttpRequest request, string relativePath)
        {
            return request.Scheme + "://" + request.Host.Value + relativePath;
        }

        
    }
}
