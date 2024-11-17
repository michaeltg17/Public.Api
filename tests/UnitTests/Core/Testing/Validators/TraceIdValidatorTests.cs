using Core.Testing;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Testing.Validators
{
    public class TraceIdValidatorTests
    {
        [Theory]
        [InlineData("00-bc43ec34fc2707cab2c1477979967041-146d776ead891946-00", true)] // Valid
        [InlineData("00-00000000000000000000000000000000-146d776ead891946-00", false)] // All zeros in Trace ID
        [InlineData("00-bc43ec34fc2707cab2c1477979967041-0000000000000000-00", false)] // All zeros in Parent ID
        [InlineData("01-bc43ec34fc2707cab2c1477979967041-146d776ead891946-00", false)] // Invalid version
        [InlineData("00-bc43ec34fc2707cab2c1477979967041-146d776ead891946-0", false)]  // Invalid flags length
        [InlineData("00-bc43ec34fc2707cab2c1477979967041-146d776ead891946-00-extra", false)] // Extra characters
        public void ValidateTraceId(string traceId, bool expected)
        {
            TraceIdValidator.IsValid(traceId).Should().Be(expected);
        }
    }
}
