using Xunit.Abstractions;

namespace FunctionalTests2
{
    public class TestOutputHelperProvider
    {
        ITestOutputHelper? TestOutputHelper;
        public ITestOutputHelper? Get() => TestOutputHelper;
        public void Set(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;
    }
}
