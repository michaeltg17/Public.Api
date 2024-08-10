namespace IntegrationTests.Settings
{
    public interface ITestSettings
    {
        public bool KeepAliveDatabase { get; }
        public bool ShouldDeployDacpac { get; }
        public bool EnableSqlLogging { get; }
    }
}
