using System.Collections.Generic;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class StateStorageViewModel : ViewModelBase, IStateStorageService
{
    private readonly HashSet<int> _viewedIds;

    public StateStorageViewModel()
    {
        _viewedIds = new HashSet<int>();
    }

    public bool GetIsViewed(int id)
    {
        return _viewedIds.Contains(id);
    }

    public void SetIsViewed(int id)
    {
        if (!_viewedIds.Contains(id))
        {
            _viewedIds.Add(id);
        }
    }
}
