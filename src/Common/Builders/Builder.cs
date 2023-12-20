namespace Common.Builders
{
    public abstract class Builder<T> : IBuilder<T>
    {
        protected abstract T Item { get; set; }

        public T Build() => Item;
    }
}
