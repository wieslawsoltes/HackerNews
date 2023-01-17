using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class UserViewModel : ViewModelBase
{
    private User _user;
    [ObservableProperty] private string _id;
    [ObservableProperty] private int _created;
    [ObservableProperty] private int _karma;
    [ObservableProperty] private string _about;
    [ObservableProperty] private List<int> _submitted;

    public UserViewModel(User user)
    {
        _user = user;
    }

    public void Update()
    {
        Id = _user.Id;
        Created = _user.Created;
        Karma = _user.Karma;
        About = _user.About;
        // TODO:
    }
}
