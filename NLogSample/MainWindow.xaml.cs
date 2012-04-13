using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NLog;

namespace NLogSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string TargetName = "memoryex";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
            SizeChanged +=  MainWindowSizeChanged;

            var target =
                LogManager.Configuration.AllTargets
                .Where(x => x.Name == TargetName)
                .Single() as MemoryTargetEx;

            if(target != null)
                target.Messages.Subscribe(msg => _messages.Add(msg));
        }

        static void MainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
           Logger.Info("Window Size Changed; New size: {0}, {1}", e.NewSize.Height, e.NewSize.Width);
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            IncomingMessages = _messages;
            Logger.Info("Window is loaded");
            Logger.Info("These messages are logged in ..\\..\\Logs\\NLogSamples.log as well.");
            Logger.Info("I will log when I am resized");
        }

        public ObservableCollection<string> IncomingMessages
        {
            get { return _messages; }
            private set { _messages = value; }
        }
    }
}
