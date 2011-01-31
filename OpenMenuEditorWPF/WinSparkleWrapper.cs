namespace OpenMenuEditorWPF {
  using System;
  using System.Runtime.InteropServices;

  public class WinSparkleWrapper
  {
    // Note that some of these functions are not implemented by WinSparkle YET.
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_init();
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_cleanup();
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_set_appcast_url(string url);
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_set_app_details(string company_name, string app_name, string app_version);
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_set_registry_path(string path);
    [DllImport("WinSparkle.dll")]
    private static extern void win_sparkle_check_update_with_ui();


    public static void Init() {
      win_sparkle_init();
    }

    public static void Cleanup() {
      win_sparkle_cleanup();
    }

    public static void SetAppcastUrl(string url) {
      win_sparkle_set_appcast_url(url);
    }

    public static void Check4Update() {
      win_sparkle_check_update_with_ui();
    }

    public static void Startup(string appcastUrl) {
      SetAppcastUrl(appcastUrl);
      Init();
      Check4Update();
    }
  }
}