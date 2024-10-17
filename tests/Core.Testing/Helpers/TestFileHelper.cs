using Core.Extensions;
using System.Diagnostics;

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

        public static string GetTestFilePath(Type type, string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, GetNamespaceAsPath(type), fileName);
        }

        public static async Task OpenFile(byte[] file, string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileExtension = Path.GetExtension(fileName);
            var finalFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid().ToString()[..8]}{fileExtension}";

            var directory = Path.GetTempPath();
            var filePath = Path.Combine(directory, finalFileName);
            await File.WriteAllBytesAsync(filePath, file);

            Process.Start("explorer", "\"" + filePath + "\"");
        }
    }
}
