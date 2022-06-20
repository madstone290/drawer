namespace Drawer.WebClient.Utils
{
    public static class UriUtils
    {
        /// <summary>
        /// Uri에 쿼리변수를 추가한다.
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="name">변수명</param>
        /// <param name="value">변수값</param>
        /// <returns></returns>
        public static string AddQueryParam(this string uri, string name, string value)
        {
            bool hasQuery = uri.Contains('?');

            if (hasQuery)
                return uri + "&" + name + "=" + value;
            else
                return uri + "?" + name + "=" + value;
        }

        /// <summary>
        /// 절대경로 Uri를 생성한다.
        /// </summary>
        /// <param name="request">현재 Http요청</param>
        /// <param name="relativePath">상대경로</param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request, string relativePath)
        {
            return request.Scheme + "://" + request.Host.Value + relativePath;
        }
    }


}
