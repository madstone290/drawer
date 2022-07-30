using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Services;
using Drawer.Web.Shared.Dialogs;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Linq.Expressions;

namespace Drawer.Web.Pages.Locations
{
    public partial class ZoneBatchEdit
    {
        private readonly ZoneModelValidator validator = new();

        public int TotalRowCount => ZoneList.Count;
        public bool IsDataValid => ZoneList.All(x => validator.Validate(x).IsValid);

        [Inject] public ZoneApiClient ApiClient { get; set; } = null!;
        [Inject] public WorkplaceApiClient WorkplaceApiClient { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] public ILockService LockService { get; set; } = null!;

        [Parameter] public EditMode ActionMode { get; set; }
        [Parameter] public List<ZoneModel> ZoneList { get; set; } = new List<ZoneModel>();


        protected override async Task OnInitializedAsync()
        {
            await GetWorkplaces();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("UseTableResize", "mud-table-root");
            }
        }

        private async Task<Dictionary<string, long>> GetWorkplaces()
        {
            var dict = await LockService.DoAsync<Dictionary<string, long>>(async () =>
            {
                var workplaceResponse = await WorkplaceApiClient.GetWorkplaces();

                var workplaceDict = new Dictionary<string, long>();
                if (Snackbar.CheckFail(workplaceResponse))
                {
                    foreach (var workplace in workplaceResponse.Data.WorkPlaces)
                        workplaceDict.Add(workplace.Name, workplace.Id);
                }
                return workplaceDict;
            });
            return dict;
        }

        private string Validate(ZoneModel item, Expression<Func<ZoneModel, object>> expression)
        {
            return validator.ValidateProperty(item, expression);
        }

        private void Back_Click()
        {
            NavManager.NavigateTo(Paths.ZoneHome);
        }

        private void Clear_Click()
        {
            ZoneList.Clear();
        }

        private async void ExcelDownload_Click()
        {
            await ExcelFileService.Download("Zone-Form.xlsx", new List<ZoneModel>());
        }

        async Task ExcelUpload_Click()
        {
            var newItemList = await ExcelFileService.Upload<ZoneModel>();
            ZoneList.AddRange(newItemList);
        }

        private void NewRow_Click()
        {
            ZoneList.Add(new ZoneModel());
        }

        private async Task Save_Click()
        {
            if (!IsDataValid)
            {
                Snackbar.Add("데이터가 유효하지 않습니다");
                return;
            }

            foreach(var zone in ZoneList)
            {
                var content = new CreateZoneRequest(zone.WorkPlaceId, zone.Name, zone.Note);
                var response = await ApiClient.AddZone(content);
                Snackbar.CheckSuccessFail(response);
            }

            NavManager.NavigateTo(Paths.ZoneHome);
            //if (Snackbar.CheckSuccessFail(response))
            //{
            //    NavManager.NavigateTo(Paths.ItemHome);
            //}
        }
    }
}
