using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.Locations.Components;
using Drawer.WebClient.Pages.Locations.Models;
using Drawer.WebClient.Pages.Locations.Views;
using Drawer.WebClient.Presenters;
using Drawer.WebClient.Shared;
using MudBlazor;

namespace Drawer.WebClient.Pages.Locations.Presenters
{
    public class WorkPlacesPresenter : SnackbarPresenter
    {
        private readonly IDialogService _dialogService;

        public IWorkPlacesView View { get; set; } = null!;

        public WorkPlacesPresenter(ApiClient apiClient, ISnackbar snackbar, IDialogService dialogService) : base(apiClient, snackbar)
        {
            _dialogService = dialogService;
        }

        public async Task LoadWorkPlacesAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.WorkPlaces.GetList);
            var response = await LoadAsync(new ApiRequestMessage<GetWorkPlacesResponse>(requstMessage));
            if (response.IsSuccessful && response.Data != null)
            {
                View.WorkPlaceList.Clear();
                foreach (var item in response.Data.WorkPlaces)
                {
                    var workPlaceModel = new WorkPlaceModel()
                    {
                        Id = item.Id,
                        Name= item.Name,
                        Description = item.Description ?? string.Empty
                    };
                    View.WorkPlaceList.Add(workPlaceModel);
                }
                RefreshTotalRowCount();
            }
        }

        public async Task ShowAddDialog()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditWorkPlaceDialog.ActionMode), ActionMode.Add },
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
                    Description = resultData.Description ?? string.Empty
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

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditWorkPlaceDialog.ActionMode), ActionMode.Update },
                { nameof(EditWorkPlaceDialog.Model), new WorkPlaceModel()
                    {
                        Id = View.SelectedWorkPlace.Id,
                        Name = View.SelectedWorkPlace.Name,
                        Description = View.SelectedWorkPlace.Description,
                    }
                }
            };
            var dialog = _dialogService.Show<EditWorkPlaceDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var workPlace = (WorkPlaceModel)result.Data;
                View.SelectedWorkPlace.Name = workPlace.Name;
                View.SelectedWorkPlace.Description = workPlace.Description;
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
                var requstMessage = new HttpRequestMessage(HttpMethod.Delete, 
                    ApiRoutes.WorkPlaces.Delete.Replace("{id}", $"{selectedItem.Id}"));
                var apiResponse = await DeleteAsync(new ApiRequestMessage(requstMessage));
                if (apiResponse.IsSuccessful)
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


