using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Components;
using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Pages.Items.Views;
using Drawer.Web.Presenters;
using Drawer.Web.Shared;
using MudBlazor;

namespace Drawer.Web.Pages.Items.Presenters
{
    public class ItemPresenter : SnackbarPresenter
    {
        private readonly ItemApiClient _itemApiClient;
        private readonly IDialogService _dialogService;

        public ItemPresenter(ISnackbar snackbar, ItemApiClient itemApiClient, IDialogService dialogService) : base(snackbar)
        {
            _itemApiClient = itemApiClient;
            _dialogService = dialogService;
        }

        public IItemView View { get; set; } = null!;


        public async Task LoadItemListAsync()
        {
            View.IsTableLoading = true;
            var response = await _itemApiClient.GetItems();
            if (!CheckFail(response))
                return;

            View.ItemList.Clear();

            foreach (var itemDto in response.Data.Items)
            {
                var item = new ItemTableModel()
                {
                    Id = itemDto.Id,
                    Name = itemDto.Name,
                    Code = itemDto.Code ?? string.Empty,
                    Number = itemDto.Number ?? string.Empty,
                    Sku = itemDto.Sku ?? string.Empty,
                    QuantityUnit = itemDto.MeasurementUnit ?? string.Empty,
                };
                View.ItemList.Add(item);
            }

            RefreshTotalRowCount();
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
                { nameof(EditItemDialog.ActionMode), ActionMode.Add },
            };
            var dialog = _dialogService.Show<EditItemDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var itemDto = (ItemModel)result.Data;
                var item = new ItemTableModel()
                {
                    Id = itemDto.Id,
                    Name = itemDto.Name,
                    Code = itemDto.Code,
                    Number = itemDto.Number,
                    Sku = itemDto.Sku,
                    QuantityUnit = itemDto.QuantityUnit
                };
                View.ItemList.Add(item);
                RefreshTotalRowCount();
            }
        } 

        public async Task ShowUpdateDialog()
        {
            var selectedItem = View.SelectedItem;
            if (selectedItem == null)
            {
                _snackbar.Add("아이템을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(EditItemDialog.ActionMode), ActionMode.Update },
                { nameof(EditItemDialog.Item), new ItemModel()
                    {
                        Id = selectedItem.Id,
                        Name = selectedItem.Name,
                        Code = selectedItem.Code,
                        Number = selectedItem.Number,
                        Sku = selectedItem.Sku,
                        QuantityUnit = selectedItem.QuantityUnit
                    }
                }
            };
            var dialog = _dialogService.Show<EditItemDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var itemDto = (ItemModel)result.Data;
                selectedItem.Name = itemDto.Name;
                selectedItem.Code = itemDto.Code;
                selectedItem.Number = itemDto.Number;
                selectedItem.Sku = itemDto.Sku;
                selectedItem.QuantityUnit = itemDto.QuantityUnit;
            }
        }

        public async Task ShowDeleteDialog()
        {
            var selectedItem = View.SelectedItem;
            if (selectedItem == null)
            {
                _snackbar.Add("아이템을 먼저 선택하세요", Severity.Normal);
                return;
            }
            
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedItem.Name} 아이템을 삭제하시겠습니까?" }
            };
            var dialog = _dialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _itemApiClient.DeleteItem(selectedItem.Id);
                CheckSuccessFail(response);

                if (response.IsSuccessful)
                {
                    View.ItemList.Remove(selectedItem);
                    RefreshTotalRowCount();
                }
            }
        }

        public async Task ShowBatchEditDialog()
        {
            var dialogOptions = new DialogOptions()
            {
                FullScreen = true
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(ItemBatchEdit.ActionMode), ActionMode.Add },
            };
            var dialog = _dialogService.Show<ItemBatchEdit>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadItemListAsync();
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
            View.TotalRowCount = View.ItemList.Count;
        }
    }
}
