using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WFInfo.Services;
using WFInfo.Settings;
using Windows.Foundation.Metadata;

namespace WFInfo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _services;

        public App()
        {
            _services = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
        }

        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ApplicationSettings.GlobalReadonlySettings);
            services.AddSoundNotification();
            services.AddOCR();
            services.AddJWTEncryption();
            services.AddProcessFinder();
            services.AddWin32WindowInfo();
            services.AddHDRDetection();

            services.AddGDIScreenshots();

            // Only add windows capture on supported plarforms (W10+ 2004 / Build 20348 and above)
            if (ApiInformation.IsTypePresent("Windows.Graphics.Capture.GraphicsCaptureSession") && ApiInformation.IsPropertyPresent("Windows.Graphics.Capture.GraphicsCaptureSession", "IsBorderRequired"))
            {
                services.AddWindowsCaptureScreenshots();
            }

            return services;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (WFInfo.MainWindow.INSTANCE != null)
            {
                WFInfo.MainWindow.listener.Dispose();
                WFInfo.MainWindow.INSTANCE.Exit(null, null);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = ActivatorUtilities.CreateInstance<MainWindow>(_services);
            mainWindow.Show();
        }
    }
}