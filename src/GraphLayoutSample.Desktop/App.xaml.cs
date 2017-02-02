using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ITCC.Logging.Core;
using ITCC.Logging.Core.Loggers;
using ITCC.Logging.Windows.Loggers;
using ITCC.UI.Loggers;
using ITCC.WPF.Windows;

namespace GraphLayoutSample.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window GetMainWindow() => Current.Windows.OfType<MainWindow>().First();

        #region ui thread access

        public static async Task RunOnUiThreadAsync(Action action) => await Current.Dispatcher.InvokeAsync(action);

        public static void RunOnUiThread(Action action) => Current.Dispatcher.Invoke(action);

        #endregion

        #region lifecycle

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            InitLoggers();

            var logWindow = new LogWindow(_observableLogger);
            logWindow.Show();

            Logger.LogEntry("APP", LogLevel.Info, "App started");
        }

        private async void App_OnExit(object sender, ExitEventArgs e)
        {
            await Logger.FlushAllAsync();
        }

        private async void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogException("APP", LogLevel.Critical, e.Exception);
            e.Handled = true;
            await Logger.FlushAllAsync();
        }

        #endregion

        #region log

        private void InitLoggers()
        {
            Logger.Level = LogLevel.Trace;

            var logDirectory = Environment.CurrentDirectory + "\\Log";
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            var logFile = logDirectory + $"\\{DateTime.Now:yyyy-MM-dd HH-mm-ss}.txt";
            var fileLogger = new BufferedFileLogger(logFile);
            Logger.RegisterReceiver(fileLogger);

            _observableLogger = new ObservableLogger(1000, RunOnUiThreadAsync);
            Logger.RegisterReceiver(_observableLogger);

            Logger.RegisterReceiver(new DebugLogger());
        }

        private ObservableLogger _observableLogger;

        public static void LogMessage(LogLevel level, string message) => Logger.LogEntry("APPLICATION", level, message);

        public static void LogException(LogLevel level, Exception exception) => Logger.LogException("APPLICATION", level, exception);

        #endregion
    }
}
