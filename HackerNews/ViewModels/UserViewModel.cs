using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class UserViewModel : ViewModelBase, ILazyLoadable
{
    private IHackerNewsApi? _api;
    private readonly INavigation? _navigation;
    private User? _user;

    [ObservableProperty] private string? _id;
    [ObservableProperty] private DateTimeOffset? _created;
    [ObservableProperty] private int? _karma;
    [ObservableProperty] private string? _about;
    [ObservableProperty] private ObservableCollection<ItemViewModel>? _submitted;

    public UserViewModel()
    {
    }

    public UserViewModel(IHackerNewsApi api, INavigation navigation, string id)
    {
        _api = api;
        _id = id;
        _navigation = navigation;

        LoadSubmittedCommand = new AsyncRelayCommand(async () =>
        {
            if (_navigation is { })
            {
                var submittedViewModel = new SubmittedViewModel(_api, _navigation, this);

                await _navigation.NavigateAsync(submittedViewModel);
            }
        });
    }

    public IAsyncRelayCommand? LoadSubmittedCommand { get; }

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
            Id = _user.Id;
            Created = DateTimeOffset.FromUnixTimeSeconds(_user.Created);
            Karma = _user.Karma;
            About = _user.About;
        }

        // TODO:
        await Task.Yield();
    }

    public async Task<bool> BackAsync()
    {
        // TODO:
        return await Task.FromResult(false);
    }

    public async Task LoadSubmittedAsync()
    {
        if (_api is { } && _navigation is { } && _user is { } && _user?.Submitted is { })
        {
            Submitted ??= new ObservableCollection<ItemViewModel>();
            Submitted.Clear();

            await ItemViewModel.LoadItemsAsync(Submitted, _user.Submitted, _api, _navigation);
        }
    }

    public override string? ToString()
    {
        return _id;
    }
}
