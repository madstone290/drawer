using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Components;
using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Pages.Organization.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.Organization.Presenters
{
    public class CompanyDetailPresenter : SnackbarPresenter
    {
        private readonly CompanyApiClient _apiClient;

        private readonly IDialogService _dialogService;
      
        public ICompanyDetailView View { get; set; } = null!;

        public CompanyDetailPresenter(ISnackbar snackbar, CompanyApiClient apiClient, IDialogService dialogService) : base(snackbar)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
        }

        public async Task GetCompanyDetailAsync()
        {
            var response = await _apiClient.GetCompany();
            CheckFail(response);

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
