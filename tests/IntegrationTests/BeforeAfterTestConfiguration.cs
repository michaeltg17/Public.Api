using System.Reflection;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace IntegrationTests
{
    public class BeforeAfterTestConfiguration(WebApplicationFactoryFixture webApplicationFactoryFixture, ITestOutputHelper testOutputHelper)
        : BeforeAfterTest
    {
        public override void Before(object? testClassInstance, MethodInfo methodUnderTest)
        {
            if (testClassInstance is Test test)
            {
                test.WebApplicationFactoryFixture = webApplicationFactoryFixture;
                test.TestOutputHelper = testOutputHelperAccessor.Output;
                test.Initialize();
            }
        }
    }
}
