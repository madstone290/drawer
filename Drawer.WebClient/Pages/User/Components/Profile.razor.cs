using Drawer.WebClient.Pages.User.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Drawer.WebClient.Pages.User.Views;
using Drawer.WebClient.Pages.User.Presenters;

namespace Drawer.WebClient.Pages.User.Components
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
