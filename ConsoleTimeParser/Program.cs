using System.Text.RegularExpressions;
using ConsoleTimeParser;

if (args.Length == 0)
{
    Console.WriteLine("You must supply a --file= argument.");
    Environment.Exit(1);
}

string fileName = string.Empty;
if (args[0].StartsWith("--file"))
{
    fileName = args[0].Split('=')[1];
}
else
{
    Console.WriteLine("You must supply a --file= argument.");
    Environment.Exit(1);
}

var lines = await File.ReadAllLinesAsync(fileName);
var timeEntries = new List<Entry>();

string timePattern = @"\[(.*?)\]";
string descriptionPattern = @"\] (.*)";

lines.ToList().ForEach(l =>
{
    var time = Regex.Match(l, timePattern).Groups[1].Value;
    var description = Regex.Match(l, descriptionPattern).Groups[1].Value;
    timeEntries.Add(new Entry(time, description));
});

var updatedEntries = EntryManager.ProcessTimeEntries(timeEntries);

var mergedEntries = EntryManager.MergeTimeEntriesByDescription(updatedEntries.Where(e => e.Description != "*" && e.Description != "lunch"));

mergedEntries.Keys.ToList().ForEach(k =>
{
    Console.WriteLine(k);
    mergedEntries[k]
        .Select(e => new { Date = e.StartDate.ToShortDateString(), Elapsed = e.ElapsedTime() })
        .GroupBy(e => e.Date)
        .Select(q =>
        {
            return new
            {
                Date = q.Key, TotalTime = q.Select(q => q.Elapsed).Aggregate((e1, e2) => e1 + e2)
            };
        })
        .ToList()
        .ForEach(z => Console.WriteLine($"\t{z.Date} - {z.TotalTime}"));

    Console.WriteLine("");
});