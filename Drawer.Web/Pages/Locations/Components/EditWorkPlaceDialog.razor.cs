using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Presenters;
using Drawer.Web.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Locations.Components
{
    public partial class EditWorkPlaceDialog : IEditWorkPlaceView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        public MudForm _form{ get; private set; } = null!;
        public bool _isFormValid { get; private set; }
        public WorkPlaceModelValidator Validator { get; private set; } = new();

        public string TitleIcon
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return Icons.Material.Filled.Add;
                else if (EditMode == EditMode.Update)
                    return Icons.Material.Filled.Update;
                else
                    return Icons.Material.Filled.ViewAgenda;
            }
        }

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "추가";
                else if (EditMode == EditMode.Update)
                    return "수정";
                else
                    return "보기";
            }
        }

        [Parameter]
        public WorkPlaceModel Model { get; set; } = new();
        [Parameter]
        public EditMode EditMode { get; set; }
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
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    await Presenter.AddWorkPlaceAsync();

                }
                else if (EditMode == EditMode.Update)
                {
                    await Presenter.UpdateWorkPlaceAsync();
                }

            }
        }

    }
}
