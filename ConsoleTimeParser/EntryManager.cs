namespace ConsoleTimeParser;

public class EntryManager
{
    public static List<Entry> ProcessTimeEntries(List<Entry> entries)
    {
        for (int i = 1; i < entries.Count; i++)
        {
            entries[i - 1].EndDate = entries[i].StartDate;
        }

        return entries;
    }
}