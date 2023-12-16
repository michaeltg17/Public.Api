namespace Common.Testing
{
    public static class FluentAssertionsExtensions
    {
        public static T Await<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}
