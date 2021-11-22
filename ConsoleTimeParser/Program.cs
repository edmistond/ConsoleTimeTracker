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
else if (File.Exists(args[0]))
{
    fileName = args[0];
}
else
{
    Console.WriteLine("You must supply a --file= argument.");
    Environment.Exit(1);
}

var lines = await File.ReadAllLinesAsync(fileName);
var timeEntries = new List<Entry>();

const string unifiedPattern = @"\[(.*?)\] (.*)";

lines.ToList().ForEach(l =>
{
    if (l == "") return;

    var groups = Regex.Match(l, unifiedPattern).Groups;

    // ignore lines that don't have time data. this is dirty but Good Enough for my needs.
    if (groups.Count == 1) return;

    var time = groups[1].Value;
    string description;
    var project = "";
    if (groups[2].Value.Contains('.'))
    {
        var split = groups[2].Value.Split('.');
        project = split[0];
        description = split[1];
    }
    else
    {
        description = groups[2].Value;
    }

    timeEntries.Add(new Entry(time, description, project));
});

timeEntries = EntryManager.ProcessTimeEntries(timeEntries);

var dates = timeEntries
    .Where(te => te.Description != "*")
    .Select(te => te.EntryDate)
    .Distinct()
    .OrderBy(d => d)
    .ToList();

// this is hideously inefficient, but since I'm doing one-file-per-week and rarely expect to have more than
// 50 or so entries in a file, the performance is more than acceptable.
dates.ForEach(d =>
{
    Console.WriteLine(d);
    // get all projects for a given entry date
    var projects = timeEntries
        .Where(te => te.EntryDate == d && te.Project != string.Empty)
        .Select(te => te.Project)
        .Distinct()
        .ToList();

    // summarize total time per-project for a given date
    projects.ForEach(p =>
    {
        var totalTime = timeEntries
            .Where(te => te.EntryDate == d && te.Project == p)
            .Select(te => te.ElapsedTime())
            .Aggregate((t1, t2) => t1 + t2);

        Console.WriteLine($"  {p} - {totalTime.Hours}:{totalTime.Minutes:00}");

        // also, for informational purposes, break down the entries per-project.
        timeEntries
            .Where(te => te.EntryDate == d && te.Project == p)
            .ToList()
            .ForEach(te => Console.WriteLine($"    {te.Description} - {te.ElapsedTime().Hours}:{te.ElapsedTime().Minutes:00}"));
    });

    Console.WriteLine("");
});
