using FluentValidation;
using System.Linq.Expressions;

namespace Drawer.Web.Utils
{
    public static class ValidationUtils
    {
        /// <summary>
        /// TModel의 입력값을 검사한다.
        /// MudForm Validation에 사용한다.
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

        public static string? ValidateProperty<TModel>(this AbstractValidator<TModel> validator, TModel instance, Expression<Func<TModel, object?>> expression)
        {
            var property = string.Empty;
            if (expression.Body is MemberExpression m)
                property = m.Member.Name;
            else if (expression.Body is UnaryExpression u && u.Operand is MemberExpression mm)
                property = mm.Member.Name;

            return ValidateProperty(validator, instance, property);
        }

        public static string? ValidateProperty<TModel>(this AbstractValidator<TModel> validator, TModel instance, string property)
        {
            var context = ValidationContext<TModel>.CreateWithOptions(instance, options => options.IncludeProperties(property));
            var result = validator.Validate(context);
            if (result.IsValid)
                return null;
            return result.Errors.First().ErrorMessage;
        }
    }
}
