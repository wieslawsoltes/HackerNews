using System.ComponentModel.DataAnnotations;

namespace HackerNews.Model;

public enum ItemType
{
    [Display(Name = "job")]
    Job,
    [Display(Name = "story")]
    Story,
    [Display(Name = "comment")]
    Comment,
    [Display(Name = "poll")]
    Poll,
    [Display(Name = "pollopt")]
    PollOpt
}
