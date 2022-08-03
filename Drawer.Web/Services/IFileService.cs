using Microsoft.JSInterop;

namespace Drawer.Web.Services
{
    public interface IFileService
    {
        Task DownloadAsync(string fileName, Stream stream);
    }

    public class FileService : IFileService
    {
        private readonly IJavaScriptService _javaScriptService;

        public FileService(IJavaScriptService javaScriptService)
        {
            _javaScriptService = javaScriptService;
        }

        public async Task DownloadAsync(string fileName, Stream stream)
        {
            await _javaScriptService.DownloadFile(fileName, stream);
        }
    }
}
