using AutoFixture.Kernel;
using AutoFixture.Idioms;

namespace Common.Testing.Extensions.AutoFixture
{
    public class StringGuardClauseAssertion(ISpecimenBuilder builder) : GuardClauseAssertion(
        builder,
        new CompositeBehaviorExpectation(
            new NullReferenceBehaviorExpectation(),
            new EmptyStringBehaviorExpectation(),
            new WhiteSpaceStringBehaviorExpectation()))
    {
    }
}
