using System.Drawing;
using System.Runtime.InteropServices;
using VIrecord.Core;

namespace VIrecord.Capture;

public class ScreenCapturer : IDisposable
{
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;
    private const int SM_XVIRTUALSCREEN = 76;
    private const int SM_YVIRTUALSCREEN = 77;
    private const int SM_CXVIRTUALSCREEN = 78;
    private const int SM_CYVIRTUALSCREEN = 79;

    public static List<ScreenInfo> GetScreens()
    {
        var screens = new List<ScreenInfo>();
        int screenCount = GetSystemMetrics(80); // SM_CMONITORS

        for (int i = 0; i < screenCount; i++)
        {
            screens.Add(new ScreenInfo
            {
                Index = i,
                Name = $"Screen {i + 1}",
                Width = GetSystemMetrics(SM_CXSCREEN),
                Height = GetSystemMetrics(SM_CYSCREEN),
                Left = 0,
                Top = 0
            });
        }

        return screens;
    }

    public static Bitmap CaptureScreen(int screenIndex = 0)
    {
        int left = GetSystemMetrics(SM_XVIRTUALSCREEN);
        int top = GetSystemMetrics(SM_YVIRTUALSCREEN);
        int width = GetSystemMetrics(SM_CXVIRTUALSCREEN);
        int height = GetSystemMetrics(SM_CYVIRTUALSCREEN);

        var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CopyFromScreen(left, top, 0, 0, new Size(width, height));
        }
        return bitmap;
    }

    public static Bitmap CaptureRegion(int x, int y, int width, int height)
    {
        var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CopyFromScreen(x, y, 0, 0, new Size(width, height));
        }
        return bitmap;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}