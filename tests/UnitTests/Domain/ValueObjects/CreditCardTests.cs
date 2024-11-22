using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace UnitTests.Domain.ValueObjects
{
    public class CreditCardTests
    {
        [Fact]
        public void ValidCreditCard_CreatesSuccessfully()
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111"; // Valid Luhn number
            var expirationDate = "12/25";
            var cvc = "123";

            // Act
            var creditCard = new CreditCard(name, number, expirationDate, cvc);

            // Assert
            Assert.Equal(name, creditCard.Name);
            Assert.Equal(number, creditCard.Number);
            Assert.Equal(expirationDate, creditCard.ExpirationDate);
            Assert.Equal(cvc, creditCard.CVC);
        }

        [Theory]
        [InlineData("4111111111111111")] // Valid Luhn
        [InlineData("378282246310005")]  // Valid Luhn
        [InlineData("6011111111111117")] // Valid Luhn
        public void ValidCardNumber_PassesValidation(string validNumber)
        {
            // Arrange
            var name = "John Doe";
            var expirationDate = "12/25";
            var cvc = "123";

            // Act
            var creditCard = new CreditCard(name, validNumber, expirationDate, cvc);

            // Assert
            Assert.Equal(validNumber, creditCard.Number);
        }

        [Theory]
        [InlineData("4111111111111112")] // Invalid Luhn
        [InlineData("123")]              // Too short
        [InlineData("411111111111111111111")] // Too long
        [InlineData("abcdabcdabcdabcd")] // Non-numeric
        public void InvalidCardNumber_ThrowsArgumentException(string invalidNumber)
        {
            // Arrange
            var name = "John Doe";
            var expirationDate = "12/25";
            var cvc = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CreditCard(name, invalidNumber, expirationDate, cvc));
        }

        [Theory]
        [InlineData("12/25")]
        [InlineData("01/30")]
        [InlineData("08/29")]
        public void ValidExpirationDate_PassesValidation(string validDate)
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111";
            var cvc = "123";

            // Act
            var creditCard = new CreditCard(name, number, validDate, cvc);

            // Assert
            Assert.Equal(validDate, creditCard.ExpirationDate);
        }

        [Theory]
        [InlineData("00/25")] // Invalid month
        [InlineData("13/25")] // Invalid month
        [InlineData("12/19")] // Expired
        [InlineData("1/25")]  // Invalid format
        [InlineData("abcd")]  // Invalid format
        public void InvalidExpirationDate_ThrowsArgumentException(string invalidDate)
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111";
            var cvc = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CreditCard(name, number, invalidDate, cvc));
        }

        [Theory]
        [InlineData("123")]
        [InlineData("4567")]
        public void ValidCVC_PassesValidation(string validCVC)
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111";
            var expirationDate = "12/25";

            // Act
            var creditCard = new CreditCard(name, number, expirationDate, validCVC);

            // Assert
            Assert.Equal(validCVC, creditCard.CVC);
        }

        [Theory]
        [InlineData("12")]      // Too short
        [InlineData("12345")]   // Too long
        [InlineData("abc")]     // Non-numeric
        public void InvalidCVC_ThrowsArgumentException(string invalidCVC)
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111";
            var expirationDate = "12/25";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CreditCard(name, number, expirationDate, invalidCVC));
        }

        [Fact]
        public void InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var number = "4111111111111111";
            var expirationDate = "12/25";
            var cvc = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CreditCard("", number, expirationDate, cvc));
            Assert.Throws<ArgumentException>(() => new CreditCard("   ", number, expirationDate, cvc));
        }

        [Fact]
        public void ToString_MasksSensitiveInformation()
        {
            // Arrange
            var name = "John Doe";
            var number = "4111111111111111";
            var expirationDate = "12/25";
            var cvc = "123";

            // Act
            var creditCard = new CreditCard(name, number, expirationDate, cvc);
            var maskedString = creditCard.ToString();

            // Assert
            Assert.DoesNotContain(number[..12], maskedString); // First 12 digits are masked
            Assert.Contains(number[^4..], maskedString); // Last 4 digits are visible
            Assert.Contains(name, maskedString); // Cardholder name is visible
            Assert.DoesNotContain(cvc, maskedString); // CVC is masked
        }

        [Fact]
        public void CannotCreateInstanceWithParameterLessConstructor()
        {
            var create = () => new CreditCard();
            create.Should().Throw<InvalidOperationException>();
        }
    }
}
