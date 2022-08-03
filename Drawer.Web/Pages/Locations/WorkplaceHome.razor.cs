using Drawer.AidBlazor;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Drawer.Web.Pages.Locations
{
    public partial class WorkplaceHome 
    {
        private AidTable<WorkplaceModel> _table = null!;
        private bool _isTableLoading;
        private List<WorkplaceModel> _workplaceList = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(WorkplaceModel.Name), "이름")
            .AddColumn(nameof(WorkplaceModel.Note), "비고")
            .Build();



        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public WorkplaceApiClient ApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public WorkplaceModel? SelectedWorkPlace => _table.FocusedItem;

        public int TotalRowCount => _workplaceList.Count;
        

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterWorkPlaces(WorkplaceModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note?.Contains(searchText) == true
            || model.Name?.Contains(searchText) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;
            var response = await ApiClient.GetWorkplaces();
            if (!Snackbar.CheckFail(response))
                return;

            _workplaceList.Clear();
            foreach (var item in response.Data.Workplaces)
            {
                var workPlaceModel = new WorkplaceModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note
                };
                _workplaceList.Add(workPlaceModel);
            }
            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.WorkplaceAdd);
        }

        private void Update_Click()
        {
            if (SelectedWorkPlace == null)
            {
                Snackbar.Add("작업장을 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.WorkplaceUpdate.Replace("{id}", $"{SelectedWorkPlace.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedWorkPlace == null)
            {
                Snackbar.Add("작업장을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedWorkPlace = SelectedWorkPlace;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedWorkPlace.Name} 작업장을 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ApiClient.DeleteWorkplace(selectedWorkPlace.Id);
                if(Snackbar.CheckSuccessFail(response))
                {
                    _workplaceList.Remove(selectedWorkPlace);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.WorkplaceBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"작업장-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _workplaceList, _excelOptions);
        }
    }
}
