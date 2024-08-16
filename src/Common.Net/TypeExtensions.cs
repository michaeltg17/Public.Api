namespace Common.Net
{
    public static class TypeExtensions
    {
        public static string GetNameWithoutGenericArity(this Type type)
        {
            var index = type.Name.IndexOf('`');
            return index == -1 ? type.Name : type.Name[..index];
        }
    }
}
