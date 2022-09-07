using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace Drawer.Web.Pages.Organization.Components
{
    public partial class JoinRequest
    {
        private readonly JoinRequestAddModel _joinRequest = new();
        private readonly JoinRequestAddModel.Validator _validator = new();
        private MudForm? _form;
        private bool _isFormValid;
        private bool _requestSent;

        [Inject]
        public CompanyJoinRequestApiClient JoinRequestApiClient { get; set; } = null!;

        private async Task Request_ClickAsync()
        {
            if (_form == null)
                return;

            await _form.Validate();

            if (!_isFormValid)
                return;

            var response = await JoinRequestApiClient.AddRequest(new JoinRequestAddCommandModel()
            {
                OwnerEmail = _joinRequest.OwnerEmail
            });

            _requestSent = Snackbar.CheckSuccessFail(response);
        }
    }
}
