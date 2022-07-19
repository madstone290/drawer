using Drawer.Web.Pages.User.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Drawer.Web.Pages.User.Views;
using Drawer.Web.Pages.User.Presenters;

namespace Drawer.Web.Pages.User.Components
{
    public partial class Profile : IProfileView
    {
        public ProfileModel Model { get; set; } = new ProfileModel();
        public ProfileModelValidator Validator { get; set; } = new ProfileModelValidator();
        public MudForm Form { get; set; } = null!;
        public bool FormIsValid { get; set; }

        [Inject] ProfilePresenter Presenter { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            Presenter.View = this;
            await Presenter.LoadUserAsync();
        }

        async Task SaveClickAsync()
        {
            await Presenter.SaveUserAsync();
        }


    }
}
