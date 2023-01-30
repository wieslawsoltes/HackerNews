using System;
using System.Collections.Generic;
using System.Web;
using HackerNews.Model;
using HtmlAgilityPack;

namespace HackerNews.Converters;

public class Node
{
    public List<Node> Nodes { get; set; } = new ();
}

public class TextNode : Node
{
    public string? Text { get; set; }
}

public class ItalicNode : Node
{
    public Node? Content { get; set; }
}

public class QuoteNode : TextNode
{
}

public class ParagraphNode : Node
{
}

public class AnchorNode : Node
{
    public string? Href { get;set; }
}

public class PreNode : Node
{
}

public class CodeNode : Node
{
    public Node? Content { get; set; }
}

public static class StringConverter
{
    private static void ParseHtml(HtmlNode htmlNode, Node root, int level)
    {
        foreach (var htmlNodeDescendant in htmlNode.Descendants())
        {
            //Console.WriteLine($"{new string(' ', level * 2)}[{htmlNodeDescendant.Name}]");
            var name = htmlNodeDescendant.Name;
            switch (name)
            {
                case "#text":
                {
                    var innerText = HttpUtility.HtmlDecode(htmlNodeDescendant.InnerText);
                    if (string.IsNullOrEmpty(innerText))
                    {
                        break;
                    }

                    if (innerText.StartsWith('>'))
                    {
                        var quote = new QuoteNode
                        {
                            Text = innerText.TrimStart('>').TrimStart()
                        };
                        root.Nodes.Add(quote);
                    }
                    else
                    {
                        var text = new TextNode
                        {
                            Text = innerText
                        };
                        root.Nodes.Add(text);
                    }

                    break;
                }
                case "p":
                {
                    var p = new ParagraphNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, p, level + 1);
                    }
                    root.Nodes.Add(p);
                    break;
                }
                case "a":
                {
                    var href = HttpUtility.HtmlDecode(htmlNodeDescendant.GetAttributeValue("href", null));
                    var a = new AnchorNode
                    {
                        Href = href
                    };
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, a, level + 1);
                    }
                    root.Nodes.Add(a);
                    break;
                }
                case "i":
                {
                    var i = new ItalicNode();

                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, i, level + 1);
                    }

                    root.Nodes.Add(i);
                    break;
                }
                case "pre":
                {
                    var pre = new PreNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, pre, level + 1);
                    }
                    root.Nodes.Add(pre);
                    break;
                }
                case "code":
                {
                    var code = new CodeNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, code, level + 1);
                    }
                    root.Nodes.Add(code);
                    break;
                }
                default:
                {
                    //Console.WriteLine($"{new string(' ', level * 2)}Unknown node name: {name}.");
                    break;
                }
            }
        }
    }

    private static void PrintNode(Node node, int level)
    {
        Console.WriteLine($"{new string(' ', level * 2)}{node.GetType().Name}");

        foreach (var childNode in node.Nodes)
        {
            PrintNode(childNode, level + 1);
        }
    }

    public static string? ParseHtmlString(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return html;
        }


        var root = new Node();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        //Console.WriteLine("");
        ParseHtml(doc.DocumentNode, root, 0);
        PrintNode(root, 0);
        
        
        

        // TODO: Remove below old code.

        var decoded = HttpUtility.HtmlDecode(html);

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
