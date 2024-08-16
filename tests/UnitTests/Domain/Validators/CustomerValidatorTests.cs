using Xunit;
using Domain.Validators;
using Domain.Models;
using FluentValidation.TestHelper;

namespace UnitTests.Domain.Validators
{
    public class CustomerValidatorTests
    {
        readonly CustomerValidator validator = new();

        public static TheoryData<Customer, bool> GetTestCases()
        {
            return new TheoryData<Customer, bool>
            {
                { new Customer { Name = "" }, false }, // Invalid: Name is empty
                { new Customer { Name = "A" }, false }, // Invalid: Name is too short
                { new Customer { Name = "A very very long name that exceeds fifty characters" }, false } // Invalid: Name is too long
                { new Customer { Name = "John" }, true }, // Valid: Name is within limits
            };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public async Task ValidateCustomer_ShouldHaveExpectedResult(Customer customer, bool isValid)
        {
            //When
            var result = await validator.TestValidateAsync(customer);

            //Then
            if (isValid)
            {
                result.ShouldNotHaveValidationErrorFor(c => c.Name);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(c => c.Name);
            }
        }
    }
}
