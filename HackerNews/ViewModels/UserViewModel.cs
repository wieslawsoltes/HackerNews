using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HackerNews.ViewModels;

public partial class UserViewModel : ViewModelBase
{
    [ObservableProperty] private string _id;
    [ObservableProperty] private int _created;
    [ObservableProperty] private int _karma;
    [ObservableProperty] private string _about;
    [ObservableProperty] private List<int> _submitted;
}
