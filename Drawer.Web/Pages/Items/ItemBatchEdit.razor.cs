using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Pages.Items.Presenters;
using Drawer.Web.Pages.Items.Views;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace Drawer.Web.Pages.Items
{
    public partial class ItemBatchEdit : IItemBatchEditView
    {
        private readonly ItemModelValidator validator = new();
        
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;

        [Inject]
        public ItemBatchEditPresenter Presenter { get; set; } = null!;

        [Parameter]
        public ActionMode ActionMode { get; set; }

        [Parameter]
        public IList<ItemModel> ItemList { get; set; } = new List<ItemModel>();

        public int TotalRowCount => ItemList.Count;

        public bool IsDataValid => ItemList.All(x => validator.Validate(x).IsValid);

        public bool IsSavingEnabled { get; set; } = true;

        protected override Task OnInitializedAsync()
        {
            Presenter.View = this;
            return base.OnInitializedAsync();
        }

        public string Validate(ItemModel item, Expression<Func<ItemModel, object>> expression)
        {
            return validator.ValidateProperty(item, expression);
        }

        private void Cancel_Click()
        {
            Dialog.Cancel();
        }

        private async Task Save_Click()
        {
            await Presenter.AddItemsAsync();
        }

        void NewRow_Click()
        {
            ItemList.Add(new ItemModel());
        }

        public void Close()
        {
            Dialog.Close();
        }
    }
}

