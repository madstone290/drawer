using Drawer.WebClient.Pages.Organization.Models;
using Drawer.WebClient.Pages.Organization.Presenters;
using Drawer.WebClient.Pages.Organization.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.WebClient.Pages.Organization.Components
{
    public partial class CompanyDetail : ICompanyDetailView
    {
        public CompanyDetailModel Model { get; set; } = new();

        [Inject]
        public CompanyDetailPresenter Presenter { get; set; } = null!;
        [Inject] 
        public IDialogService DialogService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            Presenter.View = this;
            await Presenter.GetCompanyDetailAsync();
        }

        async Task Edit_ClickAsync()
        {
            await Presenter.EditCompanyDetailAsync();
        }
    }
}
