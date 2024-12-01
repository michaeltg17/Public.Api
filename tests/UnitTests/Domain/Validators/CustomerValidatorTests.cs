using Xunit;
using Domain.Validators;
using Domain.Models;
using FluentValidation.TestHelper;
using Core.Testing.Builders;

namespace UnitTests.Domain.Validators
{
    //public class CustomerValidatorTests
    //{
    //    readonly CustomerValidator validator = new();

    //    public static TheoryData<Customer?, bool> GetTestCases()
    //    {
    //        return new TheoryData<Customer?, bool>
    //        {
    //            // Invalid: null property
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = null).Build(), false },
    //            // Invalid: Empty
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = "").Build(), false },
    //            // Invalid: Whitespace
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = "     ").Build(), false },
    //            // Invalid: Too long
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = "ESPA").Build(), false },
    //            // Invalid: Too short
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = "ES").Build(), false },
    //            // Valid
    //            { new CustomerBuilder().WithValues(c => c.CountryCode = "ESP").Build(), true }, 
    //        };
    //    }

    //    [Theory]
    //    [MemberData(nameof(GetTestCases))]
    //    public async Task ValidateCustomer_ShouldHaveExpectedResult(Customer? customer, bool isValid)
    //    {
    //        //When
    //        var result = await validator.TestValidateAsync(customer!);

    //        //Then
    //        if (isValid)
    //        {
    //            result.ShouldNotHaveValidationErrorFor(c => c.CountryCode);
    //        }
    //        else
    //        {
    //            result.ShouldHaveValidationErrorFor(c => c.CountryCode);
    //        }
    //    }
    //}
}
