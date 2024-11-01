using MoreLinq;

namespace Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> For<T>(this IEnumerable<T> items, Action<int, T> action) => For(items, 0, action);

        public static IEnumerable<T> For<T>(this IEnumerable<T> items, int i, Action<int, T> action)
        {
            var index = new IndexWrapper { Value = i };

            items.ForEach(item =>
            {
                action(index.Value, item);
                index.Value++;
            });

            return items;
        }

        class IndexWrapper
        {
            public int Value;
        }
    }
}
