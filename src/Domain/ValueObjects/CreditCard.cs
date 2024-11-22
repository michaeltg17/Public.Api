using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.ValueObjects
{
    public readonly record struct CreditCard(
        string CardNumber,
        string CardHolderName,
        DateTime ExpirationDate,
        string SecurityCode)
    {
        public string CardNumber { get; private set; } = "";
    }
}
