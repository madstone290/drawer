using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Drawer.WebClient.Utils
{
    public class JsonResult<T>
    {
        public bool IsSuccessful { get; }

        public T? Data { get; }

        public JsonResult(bool isSucessful, T? data)
        {
            IsSuccessful = isSucessful;
            Data = data;
        }
    }

    public static class JsonExtensions
    {
        /// <summary>
        /// HttpContent에서 Json역직렬화를 실행한다.
        /// Json 역직렬화가 실패할 경우 Nullable 데이터를 반환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<JsonResult<T>> ReadNullableJsonAsync<T>(this HttpContent content, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isSuccessful;
            T? data;
            try
            {
                data = await content.ReadFromJsonAsync<T>(options, cancellationToken);
                isSuccessful = true;
            }
            catch
            {
                data = default;
                isSuccessful = false;
            }
            return new JsonResult<T>(isSuccessful, data);

        }

        /// <summary>
        /// HttpContent에서 Json역직렬화를 실행한다.
        /// Json 역직렬화가 실패할 경우 Nullable 데이터를 반환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="jsonTypeInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<JsonResult<T?>> ReadNullableJsonAsync<T>(this HttpContent content, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isSuccessful;
            T? data;
            try
            {
                data = await content.ReadFromJsonAsync<T>(jsonTypeInfo, cancellationToken);
                isSuccessful = true;
            }
            catch
            {
                data = default;
                isSuccessful = false;
            }
            return new JsonResult<T?>(isSuccessful, data);
        }


    }
}
