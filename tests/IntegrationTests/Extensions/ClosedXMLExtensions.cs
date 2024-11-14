using ClosedXML.Excel;

namespace IntegrationTests.Extensions
{
    internal static class ClosedXMLExtensions
    {
        internal static object? GetValue(this IXLCell cell, Type type)
        {
            return type switch
            {
                Type t when t == typeof(Guid) => Guid.Parse(cell.Value.ToString()),
                _ => typeof(IXLCell).GetMethod(nameof(IXLCell.GetValue))!.MakeGenericMethod(type).Invoke(cell, null)
            };
        }
    }
}
