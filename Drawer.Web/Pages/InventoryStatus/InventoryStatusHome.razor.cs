using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.InventoryStatus.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.InventoryStatus
{
    public partial class InventoryStatusHome
    {
        private readonly List<TreeNode> _treeInventoryItems = new();
        private readonly List<TreeNode> _flatInventoryItems = new();

        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(ItemQtyLocationModel.ItemName), "아이템")
            .AddColumn(nameof(ItemQtyLocationModel.Quantity), "수량")
            .Build();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public ItemApiClient ItemApiClient { get; set; } = null!;
        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public InventoryItemApiClient InventoryApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public int TotalRowCount => _treeInventoryItems.Count;


        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool Filter(TreeNode node)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (node == null)
                return false;

            var rootNode = node.Root;
            
            return rootNode.InventoryItem.ItemName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                rootNode.InventoryItem.Quantity.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;

            var itemTask = ItemApiClient.GetItems();
            var locationTask = LocationApiClient.GetLocations();
            var inventoryTask= InventoryApiClient.GetInventoryDetails();
            await Task.WhenAll(itemTask, locationTask, inventoryTask);

            var itemResponse = itemTask.Result;
            var locationResponse = locationTask.Result;
            var inventoryResponse = inventoryTask.Result;

            if (!Snackbar.CheckFail(itemResponse, locationResponse, inventoryResponse))
            {
                _isTableLoading = false;
                return;
            }

            var treeNodes = new TreeNodeBuilder(itemResponse.Data, locationResponse.Data, inventoryResponse.Data)
                .Build();

            _treeInventoryItems.Clear();
            _treeInventoryItems.AddRange(treeNodes);
            
            var flatItems = Flatten(_treeInventoryItems, (i) => i.Children, 
                (p, c) => { 
                    c.Level = p.Level + 1; 
                    c.InventoryItem.ItemName = ""; 
                    c.Expanded = false; 
                });

            _flatInventoryItems.Clear();
            _flatInventoryItems.AddRange(flatItems);

            _isTableLoading = false;
        }

        private IEnumerable<T> Flatten<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> selector, Action<T, T>? beforePush = null)
        {
            var stack = new Stack<T>(source.Reverse());
            while (stack.Any())
            {
                var parent = stack.Pop();
                yield return parent;
                foreach (var child in selector(parent).Reverse())
                {
                    if(beforePush != null)
                        beforePush(parent, child);
                    stack.Push(child);
                }
            }
        }

        private void Flatten<T>(List<T> result, List<T> source, Func<T, List<T>> selector)
        {
            var stack = new Stack<T>(source);
            while (stack.Any())
            {
                var next = stack.Pop();
                result.Add(next);

                Flatten(result, selector(next), selector);
            }
        }
    
        private void Expand_Click(TreeNode node)
        {
            var expanded = !node.Expanded;
            if (expanded)
            {
                SetExpand(node, expanded, false);
            }

            if (!expanded)
            {
                SetExpand(node, expanded, true);
            }
        }

        private void SetExpand(TreeNode item, bool expanded, bool recursive = false)
        {
            item.Expanded = expanded;

            foreach(var childItem in item.Children)
            {
                SetVisible(childItem, expanded);
            }

            if (!recursive)
                return;
            
            foreach (var childItem in item.Children)
            {
                SetExpand(childItem, expanded, recursive);
            }
        }

        private void SetVisible(TreeNode from, bool visible, bool recursive = false)
        {
            from.Visible = visible;
            if (!recursive)
                return;

            foreach (var child in from.Children)
            {
                SetVisible(child, visible, recursive);
            }
        }

        private void Receipt_Click()
        {
            NavManager.NavigateTo(Paths.ReceiptHome);
        }

        private void Issue_Click()
        {
            NavManager.NavigateTo(Paths.IssueHome);
        }

        private void Transfer_Click()
        {
            NavManager.NavigateTo(Paths.IssueHome);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"재고-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _treeInventoryItems.Select(x=> x.InventoryItem), _excelOptions);
        }
    }
}
