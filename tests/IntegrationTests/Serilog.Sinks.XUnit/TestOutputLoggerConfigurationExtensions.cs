using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog;
using Xunit.Abstractions;

namespace IntegrationTests.Serilog.Sinks.XUnit
{
    public static class TestOutputLoggerConfigurationExtensions
    {
        public static LoggerConfiguration TestOutput(
            this LoggerSinkConfiguration sinkConfiguration, 
            Func<ITestOutputHelper> getTestOutputHelper, 
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, 
            string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", 
            IFormatProvider? formatProvider = null, 
            LoggingLevelSwitch? levelSwitch = null)
        {
            var textFormatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Sink(new TestOutputSink(getTestOutputHelper, textFormatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
