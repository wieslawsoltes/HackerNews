namespace HackerNews.Model;

public interface IStateManager
{
    bool GetIsViewed(int id);
    void SetIsViewed(int id);
}
