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

    public static Dictionary<string, List<Entry>> MergeTimeEntriesByDescription(IEnumerable<Entry> entries)
    {
        var mergedEntries = new Dictionary<string, List<Entry>>();
        
        entries.ToList().ForEach(e =>
        {
            List<Entry>? times;
            if (mergedEntries.TryGetValue(e.Description, out times))
            {
                mergedEntries[e.Description].Add(e);
            }
            else
            {
                mergedEntries[e.Description] = new List<Entry> { e };
            }
        });

        return mergedEntries;
    }
}