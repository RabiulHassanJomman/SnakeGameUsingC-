using Avalonia;

class Program
{
  public static AppBuilder BuildAvaloniaApp()
  {
    return AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
  }

  public static void Main(string[] arg)
  {
    BuildAvaloniaApp().StartWithClassicDesktopLifetime(arg);
  }
}