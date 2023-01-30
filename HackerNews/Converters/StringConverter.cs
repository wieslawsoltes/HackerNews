using System;
using System.Collections.Generic;
using System.Web;
using HackerNews.Model;
using HtmlAgilityPack;

namespace HackerNews.Converters;

public abstract class Node
{
    public List<Node> Nodes { get; }

    protected Node()
    {
        Nodes = new List<Node>();
    }
}

public class TextNode : Node
{
    public string Text { get; }

    public TextNode(string text)
    {
        Text = text;
    }
}

public class AnchorNode : Node
{
    public Node Content { get;  }

    public string Href { get;  }

    public AnchorNode(Node content, string href)
    {
        Content = content;
        Href = href;
    }
}

public class Layout
{
    public List<Node> Nodes { get; set; }

    public Layout()
    {
        Nodes = new List<Node>();
    }
}

public static class StringConverter
{
    public static string? ParseHtmlString(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        
        
        var doc = new HtmlDocument();
        doc.LoadHtml(text);

        Console.WriteLine($"[HtmlDocument]");
        foreach (var htmlNode in doc.DocumentNode.Descendants())
        {
            Console.WriteLine($"  {htmlNode.Name})");

            if (htmlNode.HasChildNodes)
            {
                // TODO:
            }
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
