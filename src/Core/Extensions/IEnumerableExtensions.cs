namespace Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(var item in items)
            {
                action(item);
            }

            return items;
        }

        public static IEnumerable<T> For<T>(this IEnumerable<T> items, Action<int, T> action)
        {
            var i = 0;
            items.ForEach(item =>
            {
                action(i, item);
                i++;
            });

            return items;
        }
    }
}
