using Drawer.Contract.Locations;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Models;
using Drawer.Web.Services;
using Drawer.Web.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq.Expressions;

namespace Drawer.Web.Pages.Locations
{
    public partial class SpotBatchEdit
    {
        private readonly SpotModelValidator validator = new();

        public int TotalRowCount => SpotList.Count;
        public bool IsDataValid => SpotList.All(x => validator.Validate(x).IsValid);

        [Inject] public SpotApiClient ApiClient { get; set; } = null!;
        [Inject] public ZoneApiClient ZoneApiClient { get; set; } = null!;
        [Inject] public IExcelFileService ExcelFileService { get; set; } = null!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
        [Inject] public ILockService LockService { get; set; } = null!;

        [Parameter] public EditMode ActionMode { get; set; }
        [Parameter] public List<SpotModel> SpotList { get; set; } = new List<SpotModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetZones();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("UseTableResize", "mud-table-root");
            }
        }

        private async Task<Dictionary<string, long>> GetZones()
        {
            var dict = await LockService.DoAsync<Dictionary<string, long>>(async () =>
            {
                var zoneResponse = await ZoneApiClient.GetZones();

                var zoneDict = new Dictionary<string, long>();
                if (Snackbar.CheckFail(zoneResponse))
                {
                    foreach (var Zone in zoneResponse.Data.Zones)
                        zoneDict.Add(Zone.Name, Zone.Id);
                }
                return zoneDict;
            });
            return dict;
        }

        private string Validate(SpotModel item, Expression<Func<SpotModel, object>> expression)
        {
            return validator.ValidateProperty(item, expression);
        }

        private void Back_Click()
        {
            NavManager.NavigateTo(Paths.SpotHome);
        }

        private void Clear_Click()
        {
            SpotList.Clear();
        }
        private async void ExcelDownload_Click()
        {
            await ExcelFileService.Download("Spot-Form.xlsx", new List<SpotModel>());
        }

        async Task ExcelUpload_Click()
        {
            var newItemList = await ExcelFileService.Upload<SpotModel>();
            SpotList.AddRange(newItemList);
        }

        private void NewRow_Click()
        {
            SpotList.Add(new SpotModel());
        }

        private async Task Save_Click()
        {
            if (!IsDataValid)
            {
                Snackbar.Add("데이터가 유효하지 않습니다");
                return;
            }

            foreach(var Spot in SpotList)
            {
                var content = new CreateSpotRequest(Spot.ZoneId, Spot.Name, Spot.Note);
                var response = await ApiClient.AddSpot(content);
                Snackbar.CheckSuccessFail(response);
            }

            NavManager.NavigateTo(Paths.SpotHome);
            //if (Snackbar.CheckSuccessFail(response))
            //{
            //    NavManager.NavigateTo(Paths.ItemHome);
            //}
        }
    }
}
