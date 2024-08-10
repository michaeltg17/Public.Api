namespace IntegrationTests.Settings
{
    public class TestSettings : ITestSettings
    {
        public bool KeepAliveDatabase { get; init; }
        public bool ShouldDeployDacpac { get; init; }
        public bool EnableSqlLogging { get; init; }
    }
}
