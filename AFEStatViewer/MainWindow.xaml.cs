using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Decoders = AFEStatViewer.Services.Decoders;


namespace AFEStatViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemWatcher fsw;
        private Timer saveGameChangeTimer;
        private readonly object saveGameChangeLock = new object();
        private bool saveGameLoadInProgress = false;

        public static string basePath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.AFE1_SaveGame_Path);
        public static string saveFilename = Properties.Settings.Default.AFE1_SaveGame_Filename;
        public static string saveGameFinalPath = string.Empty; // Our final result savegame path that we'll be processing.

        private ViewModels.MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void WatchForSaveGameChanges()
        {
            if (string.IsNullOrEmpty(saveGameFinalPath))
            {
                return;
            }

            string directory = System.IO.Path.GetDirectoryName(saveGameFinalPath);
            string filename = System.IO.Path.GetFileName(saveGameFinalPath);

            fsw = new FileSystemWatcher(directory, filename);

            fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

            fsw.Changed += OnSaveGameChanged;

            fsw.EnableRaisingEvents = true;
        }

        public void OnSaveGameChanged(object source, FileSystemEventArgs e)
        {
            lock (saveGameChangeLock)
            {
                saveGameChangeTimer?.Dispose();

                saveGameChangeTimer = new System.Threading.Timer(_ => ReloadSaveGameAfterChange(), null, Properties.Settings.Default.SaveGame_Change_DebounceMS, Timeout.Infinite);
            }
        }

        private async void ReloadSaveGameAfterChange()
        {
            lock (saveGameChangeLock)
            {
                if (saveGameLoadInProgress)
                {
                    return;
                }

                saveGameLoadInProgress = true;
            }

            try
            {
                var maxAttempts = Properties.Settings.Default.SaveGame_Read_MaxAttempts;
                var retryDelayMs = Properties.Settings.Default.SaveGame_Read_RetryDelayMS;

                SaveGameLoadResult lastResult = null;

                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        lastResult = LoadSavegame();
                    });

                    if (lastResult.Success)
                    {
                        return;
                    }

                    bool retryable = lastResult.Failure == SaveGameLoadFailure.ReadFailed || lastResult.Failure == SaveGameLoadFailure.DecodeFailed;

                    if (!retryable)
                    {
                        break;
                    }

                    if (attempt < maxAttempts)
                    {
                        await Task.Delay(retryDelayMs);
                    }
                }

#if DEBUG
                if (lastResult != null)
                {
                    Debug.WriteLine($"Save game reload failed: {lastResult.Failure}: {lastResult.Error}");
                }
#endif
            }
            finally
            {
                lock (saveGameChangeLock)
                {
                    saveGameLoadInProgress = false;
                }
            }
        }

        private void ApplySaveGame(string jsonString)
        {
            _vm.ApplyJson(jsonString, parseAchievements: true);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = new ViewModels.MainViewModel(new SaveGameParser());
            DataContext = _vm;

            SaveGameLoadResult result = LoadSavegame();

            if (!result.Success)
            {
                ShowSaveGameLoadError(result);
                return;
            }

            WatchForSaveGameChanges();
        }

        private void ShowSaveGameLoadError(SaveGameLoadResult result)
        {
            string title;
            MessageBoxImage icon;

            switch (result.Failure)
            {
                case SaveGameLoadFailure.SaveGameNotFound:
                    title = "Save Game Not Found";
                    icon = MessageBoxImage.Warning;
                    break;

                case SaveGameLoadFailure.ReadFailed:
                    title = "Save Game Read Error";
                    icon = MessageBoxImage.Warning;
                    break;

                case SaveGameLoadFailure.DecodeFailed:
                    title = "Save Game Decode Error";
                    icon = MessageBoxImage.Error;
                    break;

                case SaveGameLoadFailure.ApplyFailed:
                    title = "Save Game Processing Error";
                    icon = MessageBoxImage.Error;
                    break;

                default:
                    title = "Save Game Error";
                    icon = MessageBoxImage.Error;
                    break;
            }

            MessageBox.Show(result.Error, title, MessageBoxButton.OK, icon);
        }
    }
}
