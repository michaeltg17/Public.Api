using Xunit;
using Domain.Validators;
using Domain.Models;
using FluentValidation.TestHelper;
using Common.Testing.Builders;

namespace UnitTests.Domain.Validators
{
    public class CustomerValidatorTests
    {
        readonly CustomerValidator validator = new();

        public static TheoryData<Customer?, bool> GetTestCases()
        {
            return new TheoryData<Customer?, bool>
            {
                // Invalid: null
                { null, false },
                // Invalid: null property
                { new CustomerBuilder().WithValues(c => c.TestProperty = null).Build(), false },
                // Invalid: Empty
                { new CustomerBuilder().WithValues(c => c.TestProperty = "").Build(), false },
                // Invalid: Whitespace
                { new CustomerBuilder().WithValues(c => c.TestProperty = "     ").Build(), false },
                // Invalid: Too short
                { new CustomerBuilder().WithValues(c => c.TestProperty = "").Build(), false },
                // Invalid: Too long
                { new CustomerBuilder().WithValues(c => c.TestProperty = "A very very long name that exceeds fifty characters").Build(), false },
                // Valid: Name is within limits
                { new CustomerBuilder().WithValues(c => c.TestProperty = "John").Build(), true }, 
            };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public async Task ValidateCustomer_ShouldHaveExpectedResult(Customer? customer, bool isValid)
        {
            //When
            var result = await validator.TestValidateAsync(customer!);

            //Then
            if (isValid)
            {
                result.ShouldNotHaveValidationErrorFor(c => c.TestProperty);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(c => c.TestProperty);
            }
        }
    }
}
