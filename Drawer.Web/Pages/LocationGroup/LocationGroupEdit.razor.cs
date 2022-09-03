using Drawer.Application.Services.Inventory.CommandModels;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.LocationGroup.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.LocationGroup
{
    public partial class LocationGroupEdit
    {
        private MudForm? _form;
        private bool _isFormValid;
        private bool _isLoading;

        private readonly LocationGroupModel _group = new();
        private readonly LocationGroupModelValidator _validator = new();
        private readonly List<LocationGroupQueryModel> _groups = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "위치그룹 추가";
                else if (EditMode == EditMode.Update)
                    return "위치그룹 수정";
                else
                    return "위치그룹 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long LocationGroupId { get; set; }

        [Inject] public LocationGroupApiClient LocationGroupApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;

            var response = await LocationGroupApiClient.GetLocationGroups();
            if (Snackbar.CheckFail(response))
            {
                _groups.Clear();
                _groups.AddRange(response.Data);

                _validator.GroupNameList = _groups.Select(x => x.Name).ToList();
            }


            if (EditMode == EditMode.Update)
            {
                var group = _groups.FirstOrDefault(x => x.Id == LocationGroupId);
                if (group == null)
                    return;
                _group.Id = group.Id;
                _group.Name = group.Name;
                _group.Note = group.Note;
                _group.ParentGroupId = group.ParentGroupId ?? 0;
                _group.ParentGroupName = _groups.FirstOrDefault(x => x.Id == group.ParentGroupId)?.Name;
            }

            _isLoading = false;
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.LocationGroupHome);
        }

        async Task Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var groupDto = new LocationGroupAddCommandModel()
                    {
                        ParentGroupId = _group.ParentGroupId,
                        Name = _group.Name!,
                        Note = _group.Note,
                    };
                    var response = await LocationGroupApiClient.AddLocationGroup(groupDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationGroupHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var groupDto = new LocationGroupUpdateCommandModel()
                    {
                        Name = _group.Name!,
                        Note = _group.Note,
                    };
                    var response = await LocationGroupApiClient.UpdateLocationGroup(_group.Id, groupDto);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.LocationGroupHome);
                    }
                }
            }
        }

    }
}
