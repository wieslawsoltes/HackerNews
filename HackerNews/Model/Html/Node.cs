using System.Collections.Generic;
using Avalonia.Metadata;

namespace HackerNews.Model.Html;

public class Node
{
    [Content]
    public List<Node> Nodes { get; set; } = new ();
}
