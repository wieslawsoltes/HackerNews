using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class UserViewModel : ViewModelBase, ILazyLoadable
{
    private HackerNewsApiV0? _api;
    private User? _user;
    [ObservableProperty] private string? _id;
    [ObservableProperty] private int _created;
    [ObservableProperty] private int _karma;
    [ObservableProperty] private string _about;
    [ObservableProperty] private List<int> _submitted;

    public UserViewModel()
    {
    }

    public UserViewModel(HackerNewsApiV0 api, string id)
    {
        _api = api;
        _id = id;
    }

    public bool IsLoaded()
    {
        return _user is { };
    }

    public async Task LoadAsync()
    {
        if (_api is { } && _id is { })
        {
            var json = await _api.GetUserJson(_id);
            _user = await _api.DeserializeAsync<User>(json);
        }
    }

    public async Task UpdateAsync()
    {
        if (_user is { })
        {
            Created = _user.Created;
            Karma = _user.Karma;
            About = _user.About;
            // TODO:
        }

        // TODO:
        await Task.Yield();
    }

    public async Task BackAsync()
    {
        // TODO:
        await Task.Yield();
    }

    public override string? ToString()
    {
        return _id;
    }
}
