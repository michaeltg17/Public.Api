namespace Core.Extensions
{
    public enum DateTimeResolution
    {
        Year, Month, Day, Hour, Minute, Second, Millisecond, Tick
    }

    public static class DateTimeExtensions
    {
        /// <summary>
        /// <para>Truncates a DateTime to a specified resolution.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, DateTimeResolution.Second</param>
        /// <returns>Truncated DateTime</returns>
        public static DateTime Truncate(this DateTime dateTime, DateTimeResolution resolution)
        {
            return resolution switch
            {
                DateTimeResolution.Year => new DateTime(dateTime.Year, 1, 1, 0, 0, 0, 0, dateTime.Kind),
                DateTimeResolution.Month => new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Kind),
                DateTimeResolution.Day => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Kind),
                DateTimeResolution.Hour => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerHour)),
                DateTimeResolution.Minute => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMinute)),
                DateTimeResolution.Second => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond)),
                DateTimeResolution.Millisecond => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerMillisecond)),
                DateTimeResolution.Tick => dateTime,
                _ => throw new ArgumentException($"Resolution '{resolution}' not supported.", nameof(resolution)),
            };
        }
    }
}
