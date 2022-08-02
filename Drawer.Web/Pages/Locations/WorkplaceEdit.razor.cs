using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Locations
{
    public partial class WorkplaceEdit
    {
        private MudForm _form = null!;
        private bool _isFormValid;
        private readonly WorkplaceModel _workplace = new();
        private readonly WorkplaceModelValidator _validator = new();

        public string TitleText
        {
            get
            {
                if (EditMode == EditMode.Add)
                    return "작업장 추가";
                else if (EditMode == EditMode.Update)
                    return "작업장 수정";
                else
                    return "작업장 보기";
            }
        }

        public bool IsViewMode => EditMode == EditMode.View;

        [Parameter] public EditMode EditMode { get; set; }
        [Parameter] public long? WorkplaceId { get; set; }

        [Inject] public WorkplaceApiClient ApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (WorkplaceId.HasValue)
            {
                var response = await ApiClient.GetWorkplace(WorkplaceId.Value);
                if (Snackbar.CheckFail(response))
                {
                    _workplace.Id = response.Data.Id;
                    _workplace.Name = response.Data.Name;
                    _workplace.Note = response.Data.Note;
                }
            }
        }

        void Back_Click()
        {
            NavManager.NavigateTo(Paths.WorkplaceHome);
        }

        async Task Save_Click()
        {
            await _form.Validate();
            if (_isFormValid)
            {
                if (EditMode == EditMode.Add)
                {
                    var content = new CreateWorkplaceRequest(_workplace.Name!, _workplace.Note);
                    var response = await ApiClient.AddWorkplace(content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.WorkplaceHome);
                    }
                }
                else if (EditMode == EditMode.Update)
                {
                    var content = new UpdateWorkplaceRequest(_workplace.Name!,_workplace.Note);
                    var response = await ApiClient.UpdateWorkplace(_workplace.Id, content);
                    if (Snackbar.CheckSuccessFail(response))
                    {
                        NavManager.NavigateTo(Paths.WorkplaceHome);
                    }
                }

            }
        }
    }
}
