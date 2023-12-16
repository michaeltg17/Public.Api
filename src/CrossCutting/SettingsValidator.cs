using Microsoft.Extensions.Options;

namespace CrossCutting
{
    internal class SettingsValidator : IValidateOptions<Settings>
    {
        public ValidateOptionsResult Validate(string? name, Settings settings)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(settings.Url))
                validationErrors.Add($"The '{nameof(settings.Url)}' setting is required");

            if (string.IsNullOrWhiteSpace(settings.ImagesStoragePath))
                validationErrors.Add($"The '{nameof(settings.ImagesStoragePath)}' setting is required");
            else
            {
                try
                {
                    Directory.CreateDirectory(settings.ImagesStoragePath);
                }
                catch (Exception exception)
                {
                    validationErrors.Add(exception.ToString());
                }
            }

            if (string.IsNullOrWhiteSpace(settings.ImagesRequestPath))
                validationErrors.Add($"The '{nameof(settings.ImagesRequestPath)}' setting is required");

            if (string.IsNullOrWhiteSpace(settings.SqlServerConnectionString)) 
                validationErrors.Add($"The '{nameof(settings.SqlServerConnectionString)}' setting is required");

            if (validationErrors.Any()) return ValidateOptionsResult.Fail(validationErrors);

            return ValidateOptionsResult.Success;
        }
    }
}
