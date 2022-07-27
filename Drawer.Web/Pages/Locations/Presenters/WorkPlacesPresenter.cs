using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Components;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Pages.Locations.Views;
using Drawer.Web.Presenters;
using Drawer.Web.Shared;
using MudBlazor;

namespace Drawer.Web.Pages.Locations.Presenters
{
    public class WorkPlacesPresenter : SnackbarPresenter
    {
        private readonly WorkPlaceApiClient _apiClient;

        private readonly IDialogService _dialogService;

        public IWorkPlacesView View { get; set; } = null!;

        public WorkPlacesPresenter(WorkPlaceApiClient apiClient, IDialogService dialogService, ISnackbar snackbar) : base(snackbar)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
        }

        public async Task LoadWorkPlacesAsync()
        {
            View.IsTableLoading = true;
            var response = await _apiClient.GetWorkPlaces();
            CheckFail(response);

            if (response.IsSuccessful && response.Data != null)
            {
                View.WorkPlaceList.Clear();
                foreach (var item in response.Data.WorkPlaces)
                {
                    var workPlaceModel = new WorkPlaceModel()
                    {
                        Id = item.Id,
                        Name= item.Name,
                        Note = item.Note ?? string.Empty
                    };
                    View.WorkPlaceList.Add(workPlaceModel);
                }
                RefreshTotalRowCount();
            }
            View.IsTableLoading = false;
        }

        public async Task ShowAddDialog()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditWorkPlaceDialog.EditMode), EditMode.Add },
            };
            var dialog = _dialogService.Show<EditWorkPlaceDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var resultData = (WorkPlaceModel)result.Data;
                var workPlace = new WorkPlaceModel()
                {
                    Id = resultData.Id,
                    Name = resultData.Name,
                    Note = resultData.Note ?? string.Empty
                };
                View.WorkPlaceList.Add(workPlace);
                RefreshTotalRowCount();
            }
        }

        public async Task ShowUpdateDialog()
        {
            if (View.SelectedWorkPlace == null)
            {
                _snackbar.Add("작업장을 먼저 선택하세요", Severity.Normal);
                return;
            }
            var selectedItem = View.SelectedWorkPlace;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditWorkPlaceDialog.EditMode), EditMode.Update },
                { nameof(EditWorkPlaceDialog.Model), new WorkPlaceModel()
                    {
                        Id = selectedItem.Id,
                        Name = selectedItem.Name,
                        Note = selectedItem.Note,
                    }
                }
            };
            var dialog = _dialogService.Show<EditWorkPlaceDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var workPlace = (WorkPlaceModel)result.Data;
                selectedItem.Name = workPlace.Name;
                selectedItem.Note = workPlace.Note;
            }
        }

        public async Task ShowDeleteDialog()
        {
            if (View.SelectedWorkPlace == null)
            {
                _snackbar.Add("작업장을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedItem = View.SelectedWorkPlace;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"Id: {View.SelectedWorkPlace.Id} 작업장을 삭제하시겠습니까?" }
            };
            var dialog = _dialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _apiClient.DeleteWorkPlace(selectedItem.Id);
                CheckSuccessFail(response);

                if (response.IsSuccessful)
                {
                    View.WorkPlaceList.Remove(selectedItem);
                    RefreshTotalRowCount();
                }
            }
        }

        public void UploadExcel()
        {
            _snackbar.Add("구현 예정입니다");
        }

        public void DownloadExcel()
        {
            _snackbar.Add("구현 예정입니다");
        }

        private void RefreshTotalRowCount()
        {
            View.TotalRowCount = View.WorkPlaceList.Count;
        }
    }
}


