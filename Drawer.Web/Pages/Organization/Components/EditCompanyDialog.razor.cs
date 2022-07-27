using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Pages.Organization.Presenters;
using Drawer.Web.Pages.Organization.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Organization.Components
{
    public partial class EditCompanyDialog : IEditCompanyView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        
        public MudForm Form { get; private set; } = null!;
        public bool IsFormValid { get; private set; }
        public EditCompanyModelValidator Validator { get; private set; } = new ();

        public string TitleIcon
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return Icons.Material.Filled.Add;
                else if(EditMode == EditMode.Update)
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
        public EditCompanyModel Model { get; set; } = new ();
        [Parameter] 
        public EditMode EditMode { get; set; }
        [Inject]
        public EditCompanyPresenter Presenter { get; set; } = null!;

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
                if(EditMode == EditMode.Add)
                {
                    await Presenter.CreateCompanyAsync();
                }
                else if(EditMode == EditMode.Update)
                {
                     await Presenter.UpdateCompanyAsync();
                }
                
            }
        }
        
    }
}
