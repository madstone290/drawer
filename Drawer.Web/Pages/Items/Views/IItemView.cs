using Drawer.Web.Pages.Items.Models;

namespace Drawer.Web.Pages.Items.Views
{
    public interface IItemView
    {
        ItemTableModel? SelectedItem { get; }

        IList<ItemTableModel> ItemList { get; }

        int TotalRowCount { get; set; }

        bool IsTableLoading { get; set; }
    }
}
