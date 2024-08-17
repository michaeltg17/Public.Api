using Domain.Models;
using FluentValidation;

namespace Domain.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.CountryCode)
                .NotEmpty()
                .Matches("^[A-Z]{3}$");
        }
    }
}
