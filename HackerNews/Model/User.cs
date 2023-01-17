using System.Collections.Generic;

namespace HackerNews.Model;

public class User
{
    public string Id { get; set; }
    public int Created { get; set; }
    public int Karma { get; set; }
    public string About { get; set; }
    public List<int> Submitted { get; set; }
}
