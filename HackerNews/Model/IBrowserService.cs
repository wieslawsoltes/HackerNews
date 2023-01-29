using System;

namespace HackerNews.Model;

public interface IBrowserService
{
    void OpenUrl(Uri url);
}
