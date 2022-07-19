using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Presenters;
using Drawer.Web.Pages.Locations.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;

namespace Drawer.Web.Pages.Locations.Components
{
    public partial class EditSpotDialog : IEditSpotView
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;
        public MudForm Form { get; private set; } = null!;
        public bool IsFormValid { get; private set; }
        public SpotModelValidator Validator { get; private set; } = new();

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
        public SpotModel SpotModel { get; set; } = new();
        [Parameter]
        public ActionMode ActionMode { get; set; }
        [Inject]
        public EditSpotPresenter Presenter { get; set; } = null!;

        public IList<ZoneModel> ZoneModels { get; set; } = new List<ZoneModel>();


        public void CloseView()
        {
            Dialog.Close(SpotModel);
        }

        protected override async Task OnInitializedAsync()
        {
            Presenter.View = this;
            await Presenter.LoadZones();
        }

        Task<IEnumerable<long>> FilterZoneIds(string filterText)
        {
            var filteredModels = filterText == null
                ? ZoneModels
                : ZoneModels.Where(x => x.Name.Contains(filterText, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(filteredModels.Select(x=> x.Id));
        }

        string? DisplayZoneName(long id)
        {
            return ZoneModels.FirstOrDefault(x => x.Id == id)?.Name;
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
                    await Presenter.AddSpotAsync();

                }
                else if (ActionMode == ActionMode.Update)
                {
                    await Presenter.UpdateSpotAsync();
                }

            }
        }

    }
}
