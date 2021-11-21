# ConsoleTimeParser

Super-simple time tracking tool that reads data out of a text file.

Given timedata.txt:
```
[2021-11-20 09:00:00] project1.gnarfle the garthok
[2021-11-20 09:30:00] project2.planning meeting
[2021-11-20 10:00:00] project1.gnarfle the garthok
[2021-11-20 12:00:00] lunch
[2021-11-20 12:30:00] project1.gnarfle the garthok
[2021-11-20 18:00:00] *
```

And you run `ConsoleTimeParser --file=timedata.txt`, you get:

```
11/20/2021
  project1 - 8:00
    gnarfle the garthok - 0:30
    gnarfle the garthok - 2:00
    gnarfle the garthok - 5:30
  project2 - 0:30
    planning meeting - 0:30
```

You could theoretically break this down to more granularity, but
**currently** I bill everything to a single bucket of time per
customer/project and not down to the individual task level, so it's
sufficient to be able to say I spent 8 hours gnarfling the garthok
on project 1.

Times need to be `[bracketed]` so the regex will pick them up.
Projects are indicated by `projectname.` after the time. Any entries
without a project get dropped so this is mostly for taking things
like lunch or doctors' appointments out of my day.

## Roadmap

- To be written