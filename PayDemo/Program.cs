using System;
using System.IO;
using System.Reflection;
using Chromely.CefGlue;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;

namespace PayDemo
{
    public static class Program
    {
        public static IChromelyWindow MainWindow { get; private set; }

        public static int Main(string[] args)
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;

            const string startUrl = "local://dist/index.html";

            var config = ChromelyConfiguration
                    .Create()
                    .WithHostMode(WindowState.Normal)
                    .WithHostTitle("PayDemo")
//                .WithHostIconFile("")
                    .WithAppArgs(args)
                    .WithDebuggingMode(true)
                    .WithHostBounds(1200, 900)
                    .WithLogFile("logs\\chromely.cef_new.log")
                    .WithStartUrl(startUrl)
                    .WithLogSeverity(LogSeverity.Info)
                    .UseDefaultLogger("logs\\chromely_new.log")
                    .UseDefaultResourceSchemeHandler("local", string.Empty)
                    .UseDefaultHttpSchemeHandler("http", "chromely.com")
                    .WithHostFlag(HostFlagKey.CenterScreen, true)
                    .WithHostFlag(HostFlagKey.NoResize, true)
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
                    config
                        .UseDefaultSubprocess(false)
                        .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath,
                            Path.Combine(appDir, "PayDemo.Subprocess"))
                        .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, false)
                        .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                        .WithCustomSetting(CefSettingKeys.NoSandbox, false)
                        .WithCommandLineArg("disable-gpu", "1")
                        .WithCommandLineArg("disable-gpu-compositing", "1")
                        .WithCommandLineArg("disable-smooth-scrolling", "1")
                        .WithCommandLineArg("no-sandbox", "1")
                        ;
                    break;
                case PlatformID.MacOSX:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using (var window = ChromelyWindow.Create(config))
            {
                MainWindow = window;
//                MainWindow.RegisterUrlScheme(new UrlScheme("",true));
                MainWindow.RegisterServiceAssembly(Assembly.GetExecutingAssembly());
                MainWindow.ScanAssemblies();
                return MainWindow.Run(args);
            }
        }
    }
}