using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Presenters;
using Drawer.WebClient.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.WebClient.Pages.Locations.Components
{
    public partial class EditWorkPlaceDialog : IEditWorkPlaceView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        public MudForm Form { get; private set; } = null!;
        public bool IsFormValid { get; private set; }
        public WorkPlaceModelValidator Validator { get; private set; } = new();

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

        [Parameter]
        public WorkPlaceModel Model { get; set; } = new();
        [Parameter]
        public ActionMode ActionMode { get; set; }
        [Inject]
        public EditWorkPlacePresenter Presenter { get; set; } = null!;

        public void CloseView()
        {
            Dialog.Close(Model);
        }

        protected override Task OnInitializedAsync()
        {
            Presenter.View = this;

            return base.OnInitializedAsync();
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
                    await Presenter.AddWorkPlaceAsync();

                }
                else if (ActionMode == ActionMode.Update)
                {
                    await Presenter.UpdateWorkPlaceAsync();
                }

            }
        }

    }
}
