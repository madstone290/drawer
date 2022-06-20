using FluentValidation;

namespace Drawer.WebClient
{
    public static class Extensions
    {
        public static string AddQueryParam(this string uri, string name, string value)
        {
            bool hasQuery = uri.Contains('?');

            if(hasQuery)
                return uri + "&" + name + "=" + value;
            else 
                return uri + "?" + name + "=" + value;
        }

        /// <summary>
        /// TModel의 입력값을 검사한다.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<TModel>(this AbstractValidator<TModel> validator)
        {
            return async (model, propertyName) =>
            {
                var result = await validator.ValidateAsync(ValidationContext<TModel>
                    .CreateWithOptions((TModel)model, x => x.IncludeProperties(propertyName)));

                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
