using Microsoft.Extensions.Options;

namespace CrossCutting
{
    internal class SettingsValidator : IValidateOptions<Settings>
    {
        public ValidateOptionsResult Validate(string? name, Settings settings)
        {
            var failures = new List<string>();

            if (string.IsNullOrWhiteSpace(settings.ImagesPath))
                failures.Add($"The '{nameof(settings.ImagesPath)}' setting is required");
            else
            {
                try
                {
                    Directory.CreateDirectory(settings.ImagesPath);
                }
                catch (Exception exception)
                {
                    failures.Add(exception.ToString());
                }
            }

            if (string.IsNullOrWhiteSpace(settings.ImagesRequestPath))
                failures.Add($"The '{nameof(settings.ImagesRequestPath)}' setting is required");

            if (string.IsNullOrWhiteSpace(settings.SqlServerConnectionString)) 
                failures.Add($"The '{nameof(settings.SqlServerConnectionString)}' setting is required");

            if (failures.Any()) return ValidateOptionsResult.Fail(failures);

            return ValidateOptionsResult.Success;
        }
    }
}
