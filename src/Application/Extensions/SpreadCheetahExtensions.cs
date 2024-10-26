using Application.Exceptions;
using SpreadCheetah;

namespace Application.Extensions
{
    internal static class SpreadCheetahExtensions
    {
        internal static Cell ToCell(this object? @object) => @object switch
        {
            Guid guidValue => new Cell(guidValue.ToString()),
            string stringValue => new Cell(stringValue),
            ReadOnlyMemory<char> readOnlyMemoryValue => new Cell(readOnlyMemoryValue),
            int intValue => new Cell(intValue),
            long longValue => new Cell(longValue),
            float floatValue => new Cell(floatValue),
            double doubleValue => new Cell(doubleValue),
            decimal decimalValue => new Cell(decimalValue),
            bool boolValue => new Cell(boolValue),
            DateTime dateTimeValue => new Cell(dateTimeValue),
            null => new Cell(),
            _ => throw new AppException($"Unexpected object type '{@object.GetType()}'.")
        };
    }
}
