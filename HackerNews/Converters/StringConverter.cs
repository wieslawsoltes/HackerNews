using System;
using System.Web;
using HackerNews.Model;

namespace HackerNews.Converters;

public static class StringConverter
{
    public static string? ParseHtmlString(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var decoded = HttpUtility.HtmlDecode(text);

        // TODO: Parse string to text layout object tree.

        var breaks = decoded.Replace("<p>", $"{Environment.NewLine}{Environment.NewLine}");

        // TODO: Add support for <a href="">...</a> tag.
        // TODO: Add support for <i>...</i> tag.
        // TODO: Add support for <pre>...</pre> tag.
        // TODO: Add support for <code>...</code> tag.
        // TODO: Add support for '>' citation char.

        return breaks;
    }

    public static string ToTimeAgoString(DateTimeOffset dto)
    {
        var ts = DateTimeOffset.Now - dto;
        if (ts.Days > 0)
        {
            return $"{ts.Days}d";
        }
        else if (ts.Hours > 0)
        {
            return $"{ts.Hours}h";
        }
        else if (ts.Minutes > 0)
        {
            return $"{ts.Minutes}m";
        }
        else if (ts.Seconds > 0)
        {
            return $"{ts.Seconds}s";
        }
        else
        {
            return $"{ts.Milliseconds}ms";
        }
    }

    public static ItemType ItemTypeFromString(string type)
    {
        switch (type)
        {
            case "job": return ItemType.Job;
            case "story": return ItemType.Story;
            case "comment": return ItemType.Comment;
            case "poll": return ItemType.Poll;
            case "pollopt": return ItemType.PollOpt;
        }

        throw new Exception("Invalid item type.");
    }
}
