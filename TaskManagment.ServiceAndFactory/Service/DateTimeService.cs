using TaskManagment.Core.Interfaces;

namespace TaskManagment.ServiceAndFactory.Service
{
    public class DateTimeService : IDateTimeService
    {
        private static readonly TimeZoneInfo KsaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

        public DateTime ConvertToKsaTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc), KsaTimeZone);
        }
        public DateTime ConvertToUtc(DateTime ksaDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(ksaDateTime, DateTimeKind.Unspecified), KsaTimeZone);
        }
        public DateTime GetNowKsa()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, KsaTimeZone);
        }
    }

}
