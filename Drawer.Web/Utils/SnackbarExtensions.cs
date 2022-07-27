﻿using Drawer.Web.Api;
using MudBlazor;

namespace Drawer.Web.Utils
{
    public static class SnackbarExtensions
    {
        public static string SuccessMessage { get; set; } = "저장하였습니다";

        /// <summary>
        /// Api 응답이 실패한 경우 메시지를 출력한다.
        /// </summary>
        public static bool CheckFail<T>(this ISnackbar snackbar, ApiResponse<T> response)
        {
            if (!response.IsSuccessful)
            {
                snackbar.Add(response.ErrorMessage, Severity.Error);
            }
            return response.IsSuccessful;
        }

        /// <summary>
        /// Api 응답이 성공하거나 실패한 경우 메시지를 출력한다.
        /// </summary>
        public static bool CheckSuccessFail<T>(this ISnackbar snackbar, ApiResponse<T> response, string? successMessage = null)
        {
            if (response.IsSuccessful)
            {
                snackbar.Add(successMessage ?? SuccessMessage, Severity.Success);
            }

            if (!response.IsSuccessful)
            {
                snackbar.Add(response.ErrorMessage, Severity.Error);
            }
            return response.IsSuccessful;
        }
    }
}
