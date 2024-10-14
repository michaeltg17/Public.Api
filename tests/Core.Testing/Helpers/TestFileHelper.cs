using Core.Extensions;

namespace Core.Testing.Helpers
{
    public static class TestFileHelper
    {
        public static string GetNamespaceAsPath(Type type)
        {
            return type.Namespace!
                .Remove(type.Assembly.GetName().Name + ".")
                .Replace(".", @"\");
        }
    }
}
