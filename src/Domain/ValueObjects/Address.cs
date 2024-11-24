using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public readonly record struct Address
    {
        public string AddressLine { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }
        public string Country { get; }

        public Address(string addressLine, string city, string state, string postalCode, string country)
        {
            AddressLine = string.IsNullOrWhiteSpace(addressLine)
                ? throw new ArgumentException($"Invalid street. Was: '{addressLine}'", nameof(addressLine))
                : addressLine;

            City = string.IsNullOrWhiteSpace(city)
                ? throw new ArgumentException($"Invalid city. Was: '{city}'", nameof(city))
                : city;

            State = string.IsNullOrWhiteSpace(state)
                ? throw new ArgumentException($"Invalid state. Was: '{state}'", nameof(state))
                : state;

            PostalCode = IsValidPostalCode(postalCode)
                ? postalCode
                : throw new ArgumentException($"Invalid postal code. Was: '{postalCode}'", nameof(postalCode));

            Country = string.IsNullOrWhiteSpace(country)
                ? throw new ArgumentException($"Invalid country. Was: '{country}'", nameof(country))
                : country;
        }

        private static bool IsValidPostalCode(string postalCode)
        {
            // Example: Basic validation for alphanumeric postal codes
            return !string.IsNullOrWhiteSpace(postalCode) && postalCode.Length >= 3 && postalCode.Length <= 10;
        }

        public Address() => throw new InvalidConstructorException();
    }
}