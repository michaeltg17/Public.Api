using System.Text.Json;

namespace Common.Net
{
    public static class ObjectExtensions
    {
        public static string ToJson<T>(this T @object)
        {
            return JsonSerializer.Serialize(@object);
        }
    }
}
