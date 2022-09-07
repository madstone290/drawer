using FluentValidation;

namespace Drawer.Web.Pages.Organization.Models
{
    public class JoinRequestAddModel
    {
        public string OwnerEmail { get; set; } = string.Empty;
        
        public class Validator : AbstractValidator<JoinRequestAddModel>
        {
            public Validator()
            {
                RuleFor(x => x.OwnerEmail).EmailAddress();
            }
        }
    }
}
