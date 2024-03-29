﻿using Drawer.AidBlazor;
using Drawer.Application.Services.Inventory.QueryModels;
using Drawer.Web.Api.Inventory;
using Drawer.Web.Pages.Location.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Drawer.Web.Pages.Location
{
    public partial class LocationHome
    {
        private AidTable<LocationModel>? table;
        private readonly List<LocationModel> _locations = new();
        private readonly ExcelOptions _excelOptions = new ExcelOptionsBuilder()
            .AddColumn(nameof(LocationModel.GroupName), "그룹")
            .AddColumn(nameof(LocationModel.Name), "이름")
            .AddColumn(nameof(LocationModel.Note), "비고")
            .Build();

        private bool _isTableLoading;
        private bool canCreate = false;
        private bool canRead = false;
        private bool canUpdate = false;
        private bool canDelete = false;

        private string searchText = string.Empty;

        [Inject] public LocationApiClient LocationApiClient { get; set; } = null!;
        [Inject] public LocationGroupApiClient GroupApiClient { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;

        public LocationModel? SelectedLocation => table?.FocusedItem;
        public int TotalRowCount => _locations.Count;
        

        protected override async Task OnInitializedAsync()
        {
            canCreate = true;
            canRead = true;
            canUpdate = true;
            canDelete = true;

            await Load_Click();
        }

        private bool FilterLocations(LocationModel model)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;
            if (model == null)
                return false;

            return model.Note?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true|| 
                model.Name?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true||
                model.GroupName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Load_Click()
        {
            _isTableLoading = true;
            var locationTask = LocationApiClient.GetLocations();
            var groupTask = GroupApiClient.GetLocationGroups();

            await Task.WhenAll(locationTask, groupTask);

            var locationResponse = locationTask.Result;
            var groupResponse = groupTask.Result;

            if (!Snackbar.CheckFail(locationResponse, groupResponse))
            {
                _isTableLoading = false;
                return;
            }

            _locations.Clear();
            foreach (var locationDto in locationResponse.Data)
            {
                var location = new LocationModel()
                {
                    Id = locationDto.Id,
                    Name = locationDto.Name,
                    Note = locationDto.Note,
                    GroupId = locationDto.GroupId,
                    GroupName = groupResponse.Data.First(x => x.Id == locationDto.GroupId).Name,
                };
                _locations.Add(location);
            }

            _isTableLoading = false;
        }

        private void Add_Click()
        {
            NavManager.NavigateTo(Paths.LocationAdd);
        }

        private void Update_Click()
        {

            if (SelectedLocation == null)
            {
                Snackbar.Add("위치를 먼저 선택하세요", Severity.Normal);
                return;
            }
            NavManager.NavigateTo(Paths.LocationUpdate.Replace("{id}", $"{SelectedLocation.Id}"));
        }

        private async Task Delete_Click()
        {
            if (SelectedLocation == null)
            {
                Snackbar.Add("위치를 먼저 선택하세요", Severity.Normal);
                return;
            }

            var selectedLocation = SelectedLocation;

            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
                { nameof(DeleteDialog.Message), $"{selectedLocation.Name} 위치를 삭제하시겠습니까?" }
            };
            var dialog = DialogService.Show<DeleteDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await LocationApiClient.DeleteLocation(selectedLocation.Id);
                if(Snackbar.CheckSuccessFail(response))
                {
                    _locations.Remove(selectedLocation);
                }
            }
        }

        private void BatchEdit_Click()
        {
            NavManager.NavigateTo(Paths.LocationBatchEdit);
        }

        private async Task Download_ClickAsync()
        {
            var fileName = $"위치-{DateTime.Now:yyMMdd-HHmmss}.xlsx";
            await ExcelFileService.Download(fileName, _locations, _excelOptions);
        }
    }
}

