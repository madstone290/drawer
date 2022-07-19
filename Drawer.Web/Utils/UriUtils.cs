namespace Drawer.Web.Utils
{
    public static class UriUtils
    {
        /// <summary>
        /// Uri에 쿼리변수를 추가한다.
        /// URL인코딩을 적용한다.
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="name">변수명</param>
        /// <param name="value">변수값</param>
        /// <returns></returns>
        public static string AddQueryParam(this string uri, string? name, string? value)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
                return uri;

            var nameValuePair = Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value);
            bool hasQuery = uri.Contains('?');

            if (hasQuery)
                return uri + "&" + nameValuePair;
            else
                return uri + "?" + nameValuePair;
        }

        /// <summary>
        /// Uri에 추가 경로를 더한다.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AddPath(this string uri, string path)
        {
            bool hasTrailingSlash = uri.EndsWith('/');

            bool hasLeadingSlash = path.StartsWith('/');

            if (hasTrailingSlash && hasLeadingSlash)
                return uri + path.Substring(1);
            else if (hasTrailingSlash || hasLeadingSlash)
                return uri + path;
            else
                return uri + '/' + path;

            

            


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
