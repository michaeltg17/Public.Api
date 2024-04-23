using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Xunit.Abstractions;

namespace IntegrationTests.XUnit
{
    public class TestOutputSink(Func<ITestOutputHelper> getTestOutputHelper, ITextFormatter textFormatter) : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            ArgumentNullException.ThrowIfNull(logEvent);

            var renderSpace = new StringWriter();
            textFormatter.Format(logEvent, renderSpace);
            var message = renderSpace.ToString().Trim();
            getTestOutputHelper()?.WriteLine(message);
        }
    }
}
