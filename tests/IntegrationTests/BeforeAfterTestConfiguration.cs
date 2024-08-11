using System.Reflection;
using IntegrationTests.Tests;
using Xunit.DependencyInjection;

namespace IntegrationTests
{
    internal class BeforeAfterTestConfiguration(
        WebApplicationFactoryFixture webApplicationFactoryFixture, 
        ITestOutputHelperAccessor testOutputHelperAccessor)
        : BeforeAfterTest
    {
        public override void Before(object? testClassInstance, MethodInfo methodUnderTest)
        {
            if (testClassInstance is Test test)
            {
                test.WebApplicationFactoryFixture = webApplicationFactoryFixture;
                test.TestOutputHelper = testOutputHelperAccessor.Output!;
                test.Initialize();
            }
        }
    }
}
