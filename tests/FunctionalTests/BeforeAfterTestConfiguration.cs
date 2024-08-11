using System.Reflection;
using FunctionalTests.Settings;
using FunctionalTests.Tests;
using Xunit.DependencyInjection;

namespace FunctionalTests
{
    public class BeforeAfterTestConfiguration(ITestSettings testSettings) : BeforeAfterTest
    {
        public override void Before(object? testClassInstance, MethodInfo methodUnderTest)
        {
            if (testClassInstance is Test test)
            {
                test.TestSettings = testSettings;
                test.Initialize();
            }
        }
    }
}
