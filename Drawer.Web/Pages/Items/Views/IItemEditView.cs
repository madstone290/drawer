using Drawer.Web.Pages.Items.Models;

namespace Drawer.Web.Pages.Items.Views
{
    public interface IItemEditView
    {
        ItemModel Item { get; }

        void CloseView();
    }
}
