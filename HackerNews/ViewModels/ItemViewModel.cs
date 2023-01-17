using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase
{
    private Item _item;
    [ObservableProperty] private int _index;
    [ObservableProperty] private int _id;
    [ObservableProperty] private bool _deleted;
    [ObservableProperty] private string _type;
    [ObservableProperty] private UserViewModel? _by;
    [ObservableProperty] private int _time;
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _dead;
    [ObservableProperty] private int? _parent;
    [ObservableProperty] private int? _poll;
    [ObservableProperty] private List<int> _kids;
    [ObservableProperty] private string _url;
    [ObservableProperty] private int _score;
    [ObservableProperty] private string _title;
    [ObservableProperty] private List<int> _parts;
    [ObservableProperty] private int? _descendants;

    public ItemViewModel(Item item)
    {
        _item = item;
    }

    public void Update()
    {
        Index = _item.Index;
        Score = _item.Score;
        Title = _item.Title;
        // TODO:
    }
}
