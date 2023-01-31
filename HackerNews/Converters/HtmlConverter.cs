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
                    // TODO: Make QuoteNode root until next tag start.
                    // if (innerText.StartsWith('>'))
                    // {
                    //     var quote = new QuoteNode
                    //     {
                    //         Text = innerText.TrimStart('>').TrimStart()
                    //     };
                    //     root.Nodes.Add(quote);
                    // }
                    // else
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
            case TextNode textNode:
            {
                sb.Append(textNode.Text);
                break;
            }
            // TODO: Format text with quote style.
            // case QuoteNode quoteNode:
            // {
            //     sb.Append(quoteNode.Text);
            //     break;
            // }
            case ItalicNode italicNode:
            {
                // TODO: Format text with italic style.
                break;
            }
            case ParagraphNode paragraphNode:
            {
                sb.AppendLine();
                sb.AppendLine();
                break;
            }
            case AnchorNode anchorNode:
            {
                // TODO: Add anchor control.
                break;
            }
            case PreNode preNode:
            {
                // TODO: Format text with pre style.
                break;
            }
            case CodeNode codeNode:
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

    public static Node? ParseHtml(string? html)
    {
        var root = new Node();

        if (string.IsNullOrWhiteSpace(html))
        {
            return root;
        }

        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            ParseHtml(doc.DocumentNode, root);
            return root;
        }
        catch (Exception e)
        {
#if DEBUG
            Console.WriteLine(e);
#endif
            return null;
        }
    }

    public static string? ToString(Node? node)
    {
        if (node is null)
        {
            return null;
        }

        try
        {
            var sb = new StringBuilder();
            PrintNode(node, sb);
            return sb.ToString();
        }
        catch (Exception e)
        {
#if DEBUG
            Console.WriteLine(e);
#endif
            return null;
        }
    }
}
