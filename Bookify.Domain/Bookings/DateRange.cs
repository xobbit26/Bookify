namespace Bookify.Domain.Bookings;

public record DateRange
{
    private DateRange()
    {
    }

    private DateOnly Start { get; init; }
    private DateOnly End { get; init; }

    public int LengthInDays => End.DayNumber - Start.DayNumber;


    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new ApplicationException("End date precedes start date");
        }


        return new DateRange
        {
            Start = start,
            End = end
        };
    }
};