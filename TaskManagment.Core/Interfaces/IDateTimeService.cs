namespace TaskManagment.Core.Interfaces
{
    public interface IDateTimeService
    {
        DateTime ConvertToKsaTime(DateTime utcDateTime);
        DateTime ConvertToUtc(DateTime ksaDateTime);
        DateTime GetNowKsa();
    }
}
