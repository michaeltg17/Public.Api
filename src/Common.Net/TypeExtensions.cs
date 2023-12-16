namespace Common.Net
{
    public static class TypeExtensions
    {
        public static string GetNameWithoutGenericArity(this Type type)
        {
            int index = type.Name.IndexOf('`');
            return index == -1 ? type.Name : type.Name[..index];
        }
    }
}
