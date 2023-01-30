using System.Collections.Generic;

namespace HackerNews.Model.Html;

public class Node
{
    public List<Node> Nodes { get; set; } = new ();
}
