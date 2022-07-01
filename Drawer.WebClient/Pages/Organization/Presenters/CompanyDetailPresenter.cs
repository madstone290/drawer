using Drawer.Contract;
using Drawer.Contract.Organization;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.Organization.Components;
using Drawer.WebClient.Pages.Organization.Models;
using Drawer.WebClient.Pages.Organization.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.Organization.Presenters
{
    public class CompanyDetailPresenter : SnackbarPresenter
    {
        private readonly IDialogService _dialogService;

        public ICompanyDetailView View { get; set; } = null!;

        public CompanyDetailPresenter(ApiClient apiClient, ISnackbar snackbar, IDialogService dialogService)
            : base(apiClient, snackbar)
        {
            _dialogService = dialogService;
        }

        public async Task GetCompanyDetailAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Company.Get);
            var response = await LoadAsync(new ApiRequestMessage<GetCompanyResponse>(requstMessage));
            if(response.IsSuccessful && response.Data != null)
            {
                View.Model.Id = response.Data.Id;
                View.Model.Name = response.Data.Name;
                View.Model.PhoneNumber = response.Data.PhoneNumber ?? string.Empty;
            }
        }

        public async Task EditCompanyDetailAsync()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small, CloseOnEscapeKey = true,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditCompanyDialog.ActionMode), ActionMode.Update },
                { nameof(EditCompanyDialog.Model), new EditCompanyModel(View.Model.Id, View.Model.Name, View.Model.PhoneNumber) }
            };
            var dialog = _dialogService.Show<EditCompanyDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var editCompanyModel = (EditCompanyModel)result.Data;
                View.Model.Name = editCompanyModel.Name;
                View.Model.PhoneNumber = editCompanyModel.PhoneNumber;
            }
        }
    }
}
