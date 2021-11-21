namespace ConsoleTimeParser;

public class Entry
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string EntryDate { get; set; }
    public string Description { get; set; }
    public string Project { get; set; }

    public Entry(string startDate, string description, string project = "")
    {
        StartDate = DateTime.Parse(startDate);
        Description = description;
        Project = project;
        EntryDate = StartDate.ToShortDateString();
    }

    public TimeSpan ElapsedTime()
    {
        return EndDate - StartDate;
    }
}