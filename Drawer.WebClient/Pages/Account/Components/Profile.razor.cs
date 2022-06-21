using Drawer.WebClient.Pages.Account.Models;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account.Components
{
    public partial class Profile
    {
        public ProfileModel Model { get; set; } = new ProfileModel();
        public ProfileModelValidator Validator { get; set; } = new ProfileModelValidator();
        public MudForm Form { get; set; } = null!;
        public bool FormIsValid { get; set; }

        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; } = null!;

        void SaveClick()
        {

            //client.PostAsJsonAsync("/api/account/update");
        }

        protected override Task OnInitializedAsync()
        {
            var client = HttpClientFactory.CreateClient(Constants.HttpClient.DrawerApi);

            Model.DisplayName = ":tesdf";
            Model.Email = "dfmsdk@cmkdf.com";

            return base.OnInitializedAsync();
        }

    }
}
