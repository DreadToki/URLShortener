namespace ShortUrl.Helpers;

public static class DateTimeExtensions
{
    public static string FormatDateTime(this DateTime date)
        => date.ToString("F");
}
