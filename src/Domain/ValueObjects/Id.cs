using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public readonly record struct Id
    {
        public long Value { get; }

        public Id(long value)
        {
            Value = long.IsPositive(value) && value != 0
                ? value
                : throw new ArgumentException($"Invalid id. Was: '{value}'", nameof(value));
        }

        public Id() => throw new InvalidConstructorException();
    }
}
