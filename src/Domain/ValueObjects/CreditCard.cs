using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public readonly partial record struct CreditCard
    {
        public string Name { get; }
        public string Number { get; }
        public string ExpirationDate { get; }
        public string CVC { get; }

        public CreditCard(string name, string number, string expirationDate, string cvc)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException($"Invalid credit card name. Was: '{name}'", nameof(name))
                : name;

            Number = IsValidNumber(number)
                ? number
                : throw new ArgumentException($"Invalid credit card number. Was: '{number}'", nameof(number));

            ExpirationDate = IsValidExpirationDate(expirationDate)
                ? expirationDate
                : throw new ArgumentException($"Invalid credit card expiration date. Was: {expirationDate}", nameof(expirationDate));

            CVC = IsValidCVC(cvc)
                ? cvc
                : throw new ArgumentException($"Invalid credit card CVC. Was: {cvc}", nameof(cvc));
        }

        // Luhn Algorithm for card number validation
        static bool IsValidNumber(string cardNumber)
        {
            cardNumber = cardNumber.Trim();
            if (!ValidCardNumberRegex().IsMatch(cardNumber))
                return false;

            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(cardNumber[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        // Expiration date validation (Format MM/YY)
        static bool IsValidExpirationDate(string expirationDate)
        {
            if (!ValidExpirationDateRegex().IsMatch(expirationDate))
                return false;

            var now = DateTime.UtcNow;
            int month = int.Parse(expirationDate[..2]);
            int year = int.Parse("20" + expirationDate[3..]);

            var expiryDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1); // Last day of the month
            return expiryDate >= now;
        }

        static bool IsValidCVC(string cvc) => ValidCVCRegex().IsMatch(cvc);

        public override string ToString() =>
            $"CreditCard(CardNumber=**** **** **** {Number[^4..]}, " +
            $"CardholderName={Name}, " +
            $"ExpirationDate={ExpirationDate}, " +
            $"CVV=***)";

        [GeneratedRegex(@"^\d{3,4}$")]
        private static partial Regex ValidCVCRegex();

        [GeneratedRegex(@"^(0[1-9]|1[0-2])\/\d{2}$")]
        private static partial Regex ValidExpirationDateRegex();

        [GeneratedRegex(@"^\d{13,19}$")]
        private static partial Regex ValidCardNumberRegex();

        public CreditCard() => throw new InvalidOperationException("Use the parameterized constructor to create a CreditCard.");
    }
}
