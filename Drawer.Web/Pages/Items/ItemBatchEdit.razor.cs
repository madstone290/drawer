using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared.Dialogs;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;

namespace Drawer.Web.Pages.Items
{
    public partial class ItemBatchEdit
    {
        private readonly ItemModelValidator validator = new();

        public int TotalRowCount => ItemList.Count;
        public bool IsDataValid => ItemList.All(x => validator.Validate(x).IsValid);

        [Inject] public ItemApiClient ApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelService ExcelService { get; set; } = null!;
        [Inject] public IFileService FileService { get; set; } = null!;

        [Parameter] public EditMode ActionMode { get; set; }
        [Parameter] public List<ItemModel> ItemList { get; set; } = new List<ItemModel>();

        public string Validate(ItemModel item, Expression<Func<ItemModel, object>> expression)
        {
            return validator.ValidateProperty(item, expression);
        }


        private void Back_Click()
        {
            NavManager.NavigateTo(Paths.Items.Home);
        }

        private void Cancel_Click()
        {
            ItemList.Clear();
        }

        private async void ExcelDownload_Click()
        {
            var fileName = $"Item-Form.xlsx";
            var buffer = ExcelService.WriteTable(new List<ItemModel>());
            var fileStream = new MemoryStream(buffer);

            await FileService.DownloadAsync(fileName, fileStream);
        }

        async Task ExcelUpload_Click()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
            };
            var dialog = DialogService.Show<ExcelUploadDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                byte[] buffer = (byte[])result.Data;

                var newItemList = new ExcelService().ReadTable<ItemModel>(buffer);
                ItemList.AddRange(newItemList);
            }
        }

        private void NewRow_Click()
        {
            ItemList.Add(new ItemModel());
        }

        private async Task Save_Click()
        {
            if (!IsDataValid)
            {
                Snackbar.Add("데이터가 유효하지 않습니다");
                return;
            }

            foreach (var item in ItemList)
            {
                // validate
                var response = await ApiClient.AddItem(item.Name, item.Code, item.Number, item.Sku, item.QuantityUnit);
                Snackbar.CheckSuccessFail(response);
            }

            NavManager.NavigateTo(Paths.Items.Home);
        }

    }
}

