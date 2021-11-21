namespace ConsoleTimeParser;

public class Entry
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }

    public Entry(string startDate, string description)
    {
        StartDate = DateTime.Parse(startDate);
        Description = description;
    }

    public TimeSpan ElapsedTime()
    {
        return EndDate - StartDate;
    }
}