using Microsoft.Extensions.Options;

namespace CrossCutting.Settings
{
    internal class ApiSettingsValidator : IValidateOptions<ApiSettings>
    {
        public ValidateOptionsResult Validate(string? name, ApiSettings apiSettings)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(apiSettings.Url))
                validationErrors.Add($"The '{nameof(apiSettings.Url)}' setting is required");

            if (string.IsNullOrWhiteSpace(apiSettings.ImagesStoragePath))
                validationErrors.Add($"The '{nameof(apiSettings.ImagesStoragePath)}' setting is required");
            else
            {
                try
                {
                    Directory.CreateDirectory(apiSettings.ImagesStoragePath);
                }
                catch (Exception exception)
                {
                    validationErrors.Add(exception.ToString());
                }
            }

            if (string.IsNullOrWhiteSpace(apiSettings.ImagesRequestPath))
                validationErrors.Add($"The '{nameof(apiSettings.ImagesRequestPath)}' setting is required");

            if (string.IsNullOrWhiteSpace(apiSettings.SqlServerConnectionString))
                validationErrors.Add($"The '{nameof(apiSettings.SqlServerConnectionString)}' setting is required");

            if (validationErrors.Count > 0) return ValidateOptionsResult.Fail(validationErrors);

            return ValidateOptionsResult.Success;
        }
    }
}
