using Microsoft.JSInterop;

namespace Drawer.Web.Services
{
    public interface IFileService
    {
        Task DownloadAsync(string fileName, Stream stream);
    }

    public class FileService : IFileService
    {
        private readonly IJSRuntime _jsRuntime;

        public FileService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadAsync(string fileName, Stream stream)
        {
            using var streamRef = new DotNetStreamReference(stream);

            await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, streamRef);
        }
    }
}
