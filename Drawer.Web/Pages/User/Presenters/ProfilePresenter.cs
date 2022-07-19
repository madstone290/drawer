﻿using Drawer.Contract;
using Drawer.Contract.UserInformation;
using Drawer.Web.Api;
using Drawer.Web.Api.UserInformation;
using Drawer.Web.Pages.User.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.User.Presenters
{
    public class ProfilePresenter : SnackbarPresenter 
    {
        private readonly UserApiClient _apiClient;
        public ProfilePresenter(UserApiClient apiClient, ISnackbar snackbar) : base( snackbar)
        {
            _apiClient = apiClient;
        }

        public IProfileView View { get; set; } = null!;

        public async Task LoadUserAsync()
        {
            var response = await _apiClient.GetUser();
            CheckFail(response);

            if (response.IsSuccessful && response.Data != null)
            {
                View.Model.Email = response.Data.Email;
                View.Model.DisplayName = response.Data.DisplayName;
            }
        }

        public async Task SaveUserAsync()
        {
            var response = await _apiClient.SaveUser(View.Model.DisplayName!);
            CheckSuccessFail(response);
        }
    }
}