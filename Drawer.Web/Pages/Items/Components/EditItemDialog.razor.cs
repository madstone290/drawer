using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Pages.Items.Presenters;
using Drawer.Web.Pages.Items.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;

namespace Drawer.Web.Pages.Items.Components
{
    public partial class EditItemDialog : IItemEditView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        public MudForm Form { get; private set; } = null!;
        public bool IsFormValid { get; private set; }
        public ItemModelValidator Validator { get; private set; } = new();

        public string TitleIcon
        {
            get
            {
                if (ActionMode == ActionMode.Add)
                    return Icons.Material.Filled.Add;
                else if (ActionMode == ActionMode.Update)
                    return Icons.Material.Filled.Update;
                else
                    return Icons.Material.Filled.ViewAgenda;
            }
        }

        public string TitleText
        {
            get
            {
                if (ActionMode == ActionMode.Add)
                    return "추가";
                else if (ActionMode == ActionMode.Update)
                    return "수정";
                else
                    return "보기";
            }
        }

        [Inject]
        public ItemEditPresenter Presenter { get; set; } = null!;

        [Parameter]
        public ActionMode ActionMode { get; set; }

        [Parameter]
        public ItemModel Item { get; set; } = new();

        public void CloseView()
        {
            Dialog.Close(Item);
        }

        protected override Task OnInitializedAsync()
        {
            Presenter.View = this;
            return Task.CompletedTask;
        }

        void Cancel_Click()
        {
            Dialog.Cancel();
        }

        async Task Save_Click()
        {
            await Form.Validate();
            if (IsFormValid)
            {
                if (ActionMode == ActionMode.Add)
                {
                    await Presenter.AddItemAsync();
                }
                else if (ActionMode == ActionMode.Update)
                {
                    await Presenter.UpdateItemAsync();
                }

            }
        }

    }
}
