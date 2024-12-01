using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public readonly partial record struct Email
    {
        public string Address { get; }

        public Email(string address)
        {
            Address = IsValidEmail(address)
                ? address
                : throw new ArgumentException($"Invalid email address. Was: '{address}'", nameof(address));
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Simple regex for email validation
            var emailRegex = EmailRegex();
            return emailRegex.IsMatch(email);
        }

        public Email() => throw new InvalidConstructorException();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
