using Drawer.Web.DataBinding;
using FluentValidation;

namespace Drawer.Web.Pages.Locations.Models
{
    public class WorkplaceModel
    { 
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }

        /// <summary>
        /// 속성값을 초기화한다.
        /// </summary>
        public void Clear()
        {
            Id = 0;
            Name = null;
            Note = null;
        }
    }

    public class WorkplaceModelValidator : AbstractValidator<WorkplaceModel>
    {
        public WorkplaceModelValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .WithMessage("* 필수");
                 
        }
    }
}
