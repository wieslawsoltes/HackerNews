using System;
using System.Text;
using System.Web;
using HackerNews.Model.Html;
using HtmlAgilityPack;

namespace HackerNews.Converters;

public static class HtmlConverter
{
    private static void ParseHtml(HtmlNode htmlNode, Node root)
    {
        foreach (var htmlNodeDescendant in htmlNode.ChildNodes)
        {
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
                        // TODO: Make QuoteNode root until next tag start.
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
                        ParseHtml(htmlNodeDescendant, p);
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
                        ParseHtml(htmlNodeDescendant, a);
                    }
                    root.Nodes.Add(a);
                    break;
                }
                case "i":
                {
                    var i = new ItalicNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, i);
                    }
                    root.Nodes.Add(i);
                    break;
                }
                case "pre":
                {
                    var pre = new PreNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, pre);
                    }
                    root.Nodes.Add(pre);
                    break;
                }
                case "code":
                {
                    var code = new CodeNode();
                    if (htmlNodeDescendant.HasChildNodes)
                    {
                        ParseHtml(htmlNodeDescendant, code);
                    }
                    root.Nodes.Add(code);
                    break;
                }
#if DEBUG
                default:
                {
                    Console.WriteLine($"Unknown node.");
                    break;
                }
#endif
            }
        }
    }

    private static void PrintNode(Node node, StringBuilder sb)
    {
        switch (node)
        {
            case TextNode text:
            {
                sb.Append(text.Text);
                break;
            }
            case QuoteNode quote:
            {
                // TODO: Format text with quote style.
                sb.Append(quote.Text);
                break;
            }
            case ItalicNode italic:
            {
                // TODO: Format text with italic style.
                break;
            }
            case ParagraphNode paragraph:
            {
                sb.AppendLine();
                sb.AppendLine();
                break;
            }
            case AnchorNode anchor:
            {
                // TODO: Add anchor control.
                break;
            }
            case PreNode pre:
            {
                // TODO: Format text with pre style.
                break;
            }
            case CodeNode code:
            {
                // TODO: Format text with code style.
                break;
            }
        }

        foreach (var childNode in node.Nodes)
        {
            PrintNode(childNode, sb);
        }
    }

    public static string? ParseHtmlString(string? html)
    {
        // TODO: Return html object model instead of string.

        if (string.IsNullOrWhiteSpace(html))
        {
            return html;
        }

        try
        {
            var root = new Node();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            ParseHtml(doc.DocumentNode, root);
            var sb = new StringBuilder();
            PrintNode(root, sb);
            return sb.ToString();
        }
        catch (Exception e)
        {
            // TODO: Return null.
            Console.WriteLine(e);
            return $"Error: {e.Message}";
        }

        // TODO: Remove below old code.
        // var decoded = HttpUtility.HtmlDecode(html);
        // var breaks = decoded.Replace("<p>", $"{Environment.NewLine}{Environment.NewLine}");
        // return breaks;
    }
}
