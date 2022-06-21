using Drawer.WebClient.Pages.Account.Models;
using Drawer.WebClient.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.WebClient.Pages.Account
{
    public partial class Register
    {
        public RegisterModel Model { get; set; } = new RegisterModel();
        public RegisterModelValidator Validator { get; set; } = new RegisterModelValidator();
        public MudForm Form { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        async Task SubmitAsync()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                // 회원가입 진행
                var navigationUri = Paths.Account.RegisterHandler
                    .AddQueryParam("displayName", Model.DisplayName!)
                    .AddQueryParam("email", Model.Email!)
                    .AddQueryParam("password", Model.Password!);

                NavigationManager.NavigateTo(navigationUri, true);
            }
        }


    }
}
