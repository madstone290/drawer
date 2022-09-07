using Drawer.Application.Services.Organization.CommandModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Api.Organization;
using Drawer.Web.Pages.Organization.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;

namespace Drawer.Web.Pages.Organization
{
    public partial class CompanyMemberList
    {
        private readonly List<CompanyMemberModel> _companyMemberList = new();

        private bool _isLoading = false;
        private CompanyMemberModel? _selectedMemeber;


        public int MemberCount => _companyMemberList.Count;

        [Inject] public CompanyApiClient CompanyApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Load_Click();
        }

        private async Task Load_Click()
        {
            _isLoading = true;
            var memberResponse = await CompanyApiClient.GetMembers();
            
            if (!Snackbar.CheckFail(memberResponse))
            {
                _isLoading = false;
                return;
            }

            _companyMemberList.Clear();
            _companyMemberList.AddRange(memberResponse.Data.Select(x => new CompanyMemberModel()
            {
                UserId = x.UserId,
                UserEmail = x.UserEmail,
                UserName = x.UserName

            }));

            _isLoading = false;
        }

        private async Task Delete_Click()
        {
            if (_selectedMemeber == null)
                return;

            var memberResponse = await CompanyApiClient.RemoveMember(new MemberCommandModel()
            {
                UserId = _selectedMemeber.UserId,
            });
            
            if(Snackbar.CheckFail(memberResponse))
            {
                await Load_Click();
            }


        }
    }
}
