using Microsoft.JSInterop;

namespace Drawer.Web.Services
{
    /// <summary>
    /// 자바스크립트 함수를 제공한다.
    /// 모든 자바스크립트는 IJavaScriptService인터페이스에서 사용하고
    /// 다른 서비스에서 IJavaScriptService인터페이스를 참조한다.
    /// </summary>
    public interface IJavaScriptService
    {
        /// <summary>
        /// 파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadFile(string fileName, Stream stream);

        /// <summary>
        /// 테이블 리사이즈기능을 적용한다.
        /// </summary>
        /// <param name="className"></param>
        Task UseTableResize(string className);
    }

    public class JavaScriptService : IJavaScriptService
    {
        private readonly IJSRuntime _jsRuntime;

        public JavaScriptService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        public async Task DownloadFile(string fileName, Stream stream)
        {
            using var streamRef = new DotNetStreamReference(stream);
            await _jsRuntime.InvokeVoidAsync("DownloadFile", fileName, streamRef);
        }

        public async Task UseTableResize(string className)
        {
            await _jsRuntime.InvokeVoidAsync("UseTableResize", className);
        }


    }
}
