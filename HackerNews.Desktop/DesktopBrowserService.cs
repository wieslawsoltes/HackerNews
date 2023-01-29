using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HackerNews.Model;

namespace HackerNews.Desktop;

public class DesktopBrowserService : IBrowserService
{
    public async Task OpenBrowserAsync(System.Uri uri, bool external = false)
    {
        var url = uri.ToString();

        try
        {
            Process.Start(url);
        }
        catch
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

        await Task.Yield();
    }
}
