using Drawer.AidBlazor;
using Drawer.Web.Api.Items;
using Drawer.Web.Pages.Items.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Drawer.Web.Pages.Items
{
    public partial class ItemHome
    {
        private AidTable<ItemTableModel> table = null!;
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(ItemTableModel.Name), "이름")
            .AddColumn(nameof(ItemTableModel.Code), "코드")
            .AddColumn(nameof(ItemTableModel.Number), "번호")
            .AddColumn(nameof(ItemTableModel.Sku), "Sku")
            .AddColumn(nameof(ItemTableModel.QuantityUnit), "계량단위")
            .Build();


        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelService ExcelService { get; set; } = null!;
        [Inject] public IJSRuntime JS { get; set; } = null!;

        public ItemTableModel? SelectedItem => table.FocusedItem;

        public IList<ItemTableModel> ItemList { get; private set; } = new List<ItemTableModel>();

        public int TotalRowCount => ItemList.Count;

        public bool IsTableLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool Filter(ItemTableModel item)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (item == null)
                return false;

            return item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Number.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Sku.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.QuantityUnit.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task Load_Click()
        {
            IsTableLoading = true;
            var response = await ItemApiClient.GetItems();
            if (!Snackbar.CheckFail(response))
                return;

            ItemList.Clear();

            foreach (var itemDto in response.Data.Items)
            {
                var item = new ItemTableModel()
                {
                    Id = itemDto.Id,
                    Name = itemDto.Name,
                    Code = itemDto.Code ?? string.Empty,
                    Number = itemDto.Number ?? string.Empty,
                    Sku = itemDto.Sku ?? string.Empty,
                    QuantityUnit = itemDto.QuantityUnit ?? string.Empty,
                };
                ItemList.Add(item);
            }

            IsTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.ItemAdd);
        }

        private void Update_Click()
        {
            var selectedItem = SelectedItem;
            if (selectedItem == null)
            {
                Snackbar.Add("아이템을 먼저 선택하세요", Severity.Normal);
                return;
            }

            var url = Paths.ItemUpdate.Replace("{id}", $"{selectedItem.Id}");
            NavManager.NavigateTo(url);
        }

        private async Task Delete_Click()
        {
            var selectedItem = SelectedItem;
            if (selectedItem == null)
            {
                Snackbar.Add("아이템을 먼저 선택하세요", Severity.Normal);
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
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ItemApiClient.DeleteItem(selectedItem.Id);
                Snackbar.CheckSuccessFail(response);

                if (response.IsSuccessful)
                {
                    ItemList.Remove(selectedItem);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.ItemBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var buffer = ExcelService.WriteTable(ItemList, _excelOptions);
            var fileStream = new MemoryStream(buffer);
            var fileName = $"아이템-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            using var streamRef = new DotNetStreamReference(fileStream);

            await JS.InvokeVoidAsync("downloadFile", fileName, streamRef);
        }
    }
}
