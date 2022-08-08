﻿using Drawer.Contract;
using Drawer.Contract.Authentication;
using Drawer.IntergrationTest.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.IntergrationTest
{
    public static class Extensions
    {
        /// <summary>
        /// 마스터 계정으로 로그인한다
        /// </summary>
        public static async Task<LoginResponse> MasterLoginAsync(this HttpClient client)
        {
            var loginRequest = new LoginRequest(UserSeeds.Master.Email, UserSeeds.Master.Password);
            var loginResponseMessage = await client.PostAsJsonAsync(ApiRoutes.Account.Login, loginRequest);
            var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<LoginResponse>();
            return loginResponse!;
        }

        public static async Task<HttpResponseMessage> SendAsyncWithMasterAuthentication(this HttpClient client, HttpRequestMessage requestMessage)
        {
            var loginResponse = await client.MasterLoginAsync();
            requestMessage.SetBearerToken(loginResponse.AccessToken);

            return await client.SendAsync(requestMessage);
        }

        public static void SetBearerToken(this HttpRequestMessage request, string token)
        {
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"bearer {token}");
        }

        /// <summary>
        /// 두 시간이 주어진 정확도만큼 근접한지 확인한다.
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IsCloseTo(this DateTime time1, DateTime time2, TimeSpan precision)
        {
            return time1.Subtract(time2).Duration() <= precision;
        }

        public static string ToDateFormat(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

    }
}
