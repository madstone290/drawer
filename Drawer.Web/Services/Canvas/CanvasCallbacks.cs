using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Drawer.Web.Services.Canvas
{
    /// <summary>
    /// 캔버스에서 발생하는 자바스크립트 이벤트의 콜백 메소드를 등록한다.
    /// </summary>
    public class CanvasCallbacks
    {
        /// <summary>
        /// 캔버스 아이템이 선택/해제된 경우
        /// </summary>
        public EventCallback<string> OnItemSelectionChanged { get; set; }

        [JSInvokable]
        public async Task ItemSelectionChanged(string id)
        {
            await OnItemSelectionChanged.InvokeAsync(id);
        }
    }
}
