using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public readonly record struct Name
    {
        public string Value { get; }

        public Name(string value)
        {
            Value = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException($"Invalid name. Was: '{value}'", nameof(Value))
                : value;
        }

        public Name() => throw new InvalidConstructorException();
    }
}
