using Domain.Models;
using FluentValidation;

namespace Domain.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.TestProperty)
                .NotEmpty().WithMessage("TestProperty is required.")
                .MinimumLength(2).WithMessage("TestProperty must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("TestProperty cannot exceed 50 characters.");
        }
    }
}
