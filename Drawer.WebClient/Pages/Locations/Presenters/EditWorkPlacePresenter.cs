using Drawer.Contract;
using Drawer.Contract.Locations;
using Drawer.WebClient.Api;
using Drawer.WebClient.Pages.Locations.Views;
using Drawer.WebClient.Presenters;
using MudBlazor;

namespace Drawer.WebClient.Pages.Locations.Presenters
{
    public class EditWorkPlacePresenter : SnackbarPresenter
    {
      
        public IEditWorkPlaceView View { get; set; } = null!;

        public EditWorkPlacePresenter(ApiClient apiClient, ISnackbar snackbar) : base(apiClient, snackbar)
        {
        }

        /// <summary>
        /// 작업장을 추가하고 성공여부 반환한다.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddWorkPlaceAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WorkPlaces.Create);
            requstMessage.Content = JsonContent.Create(new CreateWorkPlaceRequest(View.Model.Name, View.Model.Description));

            var apiResponse = await SaveAsync(new ApiRequestMessage<CreateWorkPlaceResponse>(requstMessage));
            if(apiResponse.IsSuccessful && apiResponse.Data != null)
            {
                View.Model.Id = apiResponse.Data.Id;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 작업장을 수정하고 성공여부를 반환한다.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateWorkPlaceAsync()
        {
            var requstMessage = new HttpRequestMessage(HttpMethod.Put, 
                ApiRoutes.WorkPlaces.Update.Replace("{id}", View.Model.Id.ToString()));
            requstMessage.Content = JsonContent.Create(new UpdateWorkPlaceRequest(View.Model.Name, View.Model.Description));

            var apiResponse = await SaveAsync(new ApiRequestMessage<CreateWorkPlaceResponse>(requstMessage));
            if (apiResponse.IsSuccessful)
            {
                return true;
            }
            return false;
        }
    }
}
