using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Presenters;
using Drawer.Web.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;

namespace Drawer.Web.Pages.Locations.Components
{
    public partial class EditZoneDialog : IEditZoneView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        public MudForm Form { get; private set; } = null!;
        public bool IsFormValid { get; private set; }
        public ZoneModelValidator Validator { get; private set; } = new();

        public string TitleIcon
        {
            get
            {
                if (ActionMode == ActionMode.Add)
                    return Icons.Material.Filled.Add;
                else if (ActionMode == ActionMode.Update)
                    return Icons.Material.Filled.Update;
                else
                    return Icons.Material.Filled.ViewAgenda;
            }
        }

        public string TitleText
        {
            get
            {
                if (ActionMode == ActionMode.Add)
                    return "추가";
                else if (ActionMode == ActionMode.Update)
                    return "수정";
                else
                    return "보기";
            }
        }

        [Parameter]
        public ZoneModel Model { get; set; } = new();
        [Parameter]
        public ActionMode ActionMode { get; set; }
        [Inject]
        public EditZonePresenter Presenter { get; set; } = null!;

        public IList<WorkPlaceModel> WorkPlaceModels { get; set; } = new List<WorkPlaceModel>();


        public void CloseView()
        {
            Dialog.Close(Model);
        }

        protected override async Task OnInitializedAsync()
        {
            Presenter.View = this;
            await Presenter.LoadWorkPlaces();
        }

        Task<IEnumerable<long>> FilterWorkPlaceIds(string filterText)
        {
            var filteredModels = filterText == null
                ? WorkPlaceModels
                : WorkPlaceModels.Where(x => x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(filteredModels.Select(x=> x.Id));
        }

        string? DisplayWorkPlaceName(long id)
        {
            return WorkPlaceModels.FirstOrDefault(x => x.Id == id)?.Name;
        }

        void Cancel_Click()
        {
            Dialog.Cancel();
        }

        async Task Save_Click()
        {
            await Form.Validate();
            if (IsFormValid)
            {
                if (ActionMode == ActionMode.Add)
                {
                    await Presenter.AddZoneAsync();

                }
                else if (ActionMode == ActionMode.Update)
                {
                    await Presenter.UpdateZoneAsync();
                }

            }
        }

    }
}
