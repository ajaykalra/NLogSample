using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using NLog;

namespace NLogSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create the target

           // LogManager.Configuration.AddTarget("MemoryEx", new MemoryTargetEx());
            logger.Info("App is starting up..");

            var window = new MainWindow();
            window.DataContext = window;

            window.Show();
        }
    }
}
