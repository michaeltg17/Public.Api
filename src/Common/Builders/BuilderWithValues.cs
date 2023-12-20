namespace Common.Builders
{
    public abstract class BuilderWithValues<BuilderType, ObjectToBuildType> : Builder<ObjectToBuildType>
        where BuilderType : BuilderWithValues<BuilderType, ObjectToBuildType>
    {
        public BuilderType WithValues(Action<ObjectToBuildType> action)
        {
            action(Item);
            return (BuilderType)this;
        }
    }
}
