namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string Remove(this string thisString, string @string)
        {
            return thisString.Replace(@string, string.Empty);
        }
    }
}
