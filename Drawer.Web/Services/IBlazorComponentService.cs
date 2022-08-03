namespace Drawer.Web.Services
{
    /// <summary>
    /// 블레이저 컴포넌트에 적용할 기능을 제공한다.
    /// </summary>
    public interface IBlazorComponentService
    {
        /// <summary>
        /// 테이블 리사이즈 기능을 적용한다.
        /// </summary>
        /// <returns></returns>
        Task TableResize(string? className = null);
    }

    public class BlazorComponentService : IBlazorComponentService
    {
        private const string MUD_TABLE_CLASS = "mud-table-root";

        private readonly IJavaScriptService _javascriptService;

        public BlazorComponentService(IJavaScriptService javascriptService)
        {
            _javascriptService = javascriptService;
        }

        public async Task TableResize(string? className = null)
        {
            await _javascriptService.UseTableResize(className ?? MUD_TABLE_CLASS);
        }
    }
}
