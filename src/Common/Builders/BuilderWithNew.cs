namespace Common.Builders
{
    public abstract class BuilderWithNew<BuilderType, ObjectToBuildType> : BuilderWithValues<BuilderType, ObjectToBuildType>
        where BuilderType : BuilderWithNew<BuilderType, ObjectToBuildType>
        where ObjectToBuildType : new()
    {
        protected override ObjectToBuildType Item { get; set; } = new ObjectToBuildType();
    }
}
