using ImageSecret.Services.SteganographyService;
using ImageSecret.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSecret
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        /// <summary>
        /// LEARN DEPENDENCY INJECTION HERE
        /// </summary>
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            // Dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ISteganographyService, SteganographyService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show the main window using dependncy injection

            MainWindow mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainViewModel>();
            mainWindow?.Show();
        }
    }
}
