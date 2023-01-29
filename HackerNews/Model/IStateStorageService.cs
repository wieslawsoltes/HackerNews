namespace HackerNews.Model;

public interface IStateStorageService
{
    bool GetIsViewed(int id);
    void SetIsViewed(int id);
}
