using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public readonly partial record struct Phone
    {
        public string Number { get; }

        public Phone(string number)
        {
            Number = IsValidPhoneNumber(number)
                ? number
                : throw new ArgumentException($"Invalid phone number. Was: '{number}'", nameof(number));
        }

        private static bool IsValidPhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            // Simple regex for phone number validation (international and local formats)
            var phoneRegex = PhoneRegex();
            return phoneRegex.IsMatch(number);
        }

        public Phone() => throw new InvalidConstructorException();

        [GeneratedRegex(@"^\+?[1-9]\d{1,14}$")]
        private static partial Regex PhoneRegex();
    }
}
