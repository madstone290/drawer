using Drawer.Web.Api;
using Drawer.Web.Api.Locations;
using Drawer.Web.Pages.Locations.Views;
using Drawer.Web.Presenters;
using MudBlazor;

namespace Drawer.Web.Pages.Locations.Presenters
{
    public class EditWorkPlacePresenter : SnackbarPresenter
    {
        private readonly WorkPlaceApiClient _apiClient;

        public IEditWorkPlaceView View { get; set; } = null!;

        public EditWorkPlacePresenter(WorkPlaceApiClient apiClient, ISnackbar snackbar) : base(snackbar)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// 작업장을 추가한다.
        /// </summary>
        /// <returns></returns>
        public async Task AddWorkPlaceAsync()
        {
            var response = await _apiClient.AddWorkPlace(View.Model.Name, View.Model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful && response.Data != null)
                View.Model.Id = response.Data.Id;

            if (response.IsSuccessful)
                View.CloseView();
        }

        /// <summary>
        /// 작업장을 수정한다.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateWorkPlaceAsync()
        {
            var model = View.Model;
            var response = await _apiClient.UpdateWorkPlace(model.Id, model.Name, model.Note);
            CheckSuccessFail(response);

            if (response.IsSuccessful)
                View.CloseView();
        }
    }
}
