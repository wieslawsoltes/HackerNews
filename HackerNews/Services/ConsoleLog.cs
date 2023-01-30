using System;
using HackerNews.Model;

namespace HackerNews.Services;

public class ConsoleLog : ILog
{
    public void Log(object message)
    {
        Console.WriteLine(message);
    }
}
