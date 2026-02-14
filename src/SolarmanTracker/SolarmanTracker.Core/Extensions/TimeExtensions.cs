namespace SolarmanTracker.Core.Extensions
{
    public static class TimeExtensions
    {
        private static TimeZoneInfo KyivZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Kyiv");

        public static DateTime ToKyivTime(this DateTime time)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, KyivZone);
        }
    }
}
