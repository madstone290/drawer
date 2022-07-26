using Drawer.Web.Pages.Items.Models;

namespace Drawer.Web.Pages.Items.Views
{
    public interface IItemBatchEditView
    {
        IList<ItemModel> ItemList { get; }
        
        bool IsDataValid { get; }

        /// <summary>
        /// 저장가능 상태를 변경한다
        /// </summary>
        bool IsSavingEnabled { get; set; }

        void Close();
    }
}
