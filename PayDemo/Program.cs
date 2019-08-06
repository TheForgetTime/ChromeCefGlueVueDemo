using System;
using Chromely.CefGlue;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;

namespace PayDemo
{
    public static class Program
    {
        public static IChromelyWindow MainWindow { get; private set; }
        public static int Main(string[] args)
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;

            const string startUrl = "local://app/index.html";

            var config = ChromelyConfiguration
                .Create()
                .WithAppArgs(args)
                .WithStartUrl(startUrl)
                ;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                case PlatformID.WinCE:
                case PlatformID.Xbox:
                    config.UseDefaultSubprocess();
                    break;
                case PlatformID.Unix:
                    config.UseDefaultSubprocess(false)
                        .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, appDir + "PayDemo.Subprocess")
                        ;
                    break;
                case PlatformID.MacOSX:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using (var window=ChromelyWindow.Create(config))
            {
                MainWindow = window;
                MainWindow.ScanAssemblies();
                return MainWindow.Run(args);
            }
        }
    }
}