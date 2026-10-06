namespace Domain.Shared;

public static class DateRanges
{
    public static DateRange Month(int year, int month)
    {
        var from = new DateTime(year, month, 1);
        return new DateRange(from, from.AddMonths(1));
    }

    public static DateRange Year(int year)
    {
        var from = new DateTime(year, 1, 1);
        return new DateRange(from, from.AddYears(1));
    }

    public static DateRange Day(DateTime date)
    {
        var from = date.Date;
        return new DateRange(from, from.AddDays(1));
    }
}
