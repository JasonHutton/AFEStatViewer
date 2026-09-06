using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
        private SaveGameLoader saveGameLoader;

        private ViewModels.MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            saveGameLoader = new SaveGameLoader(
                Environment.ExpandEnvironmentVariables(
                    Properties.Settings.Default.AFE1_SaveGame_Path
                ),
                Properties.Settings.Default.AFE1_SaveGame_Filename
            );
        }

        public void WatchForSaveGameChanges()
        {
            if (string.IsNullOrEmpty(saveGameLoader.SaveGamePath))
            {
                return;
            }

            string directory = System.IO.Path.GetDirectoryName(saveGameLoader.SaveGamePath);
            string filename = System.IO.Path.GetFileName(saveGameLoader.SaveGamePath);

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
                        lastResult = saveGameLoader.LoadSavegame();

                        if (lastResult.Success)
                        {
                            ApplySaveGame(lastResult.Json);
                        }
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

            SaveGameLoadResult result = saveGameLoader.LoadSavegame();

            if (!result.Success)
            {
                ShowSaveGameLoadError(result);
                return;
            }

            ApplySaveGame(result.Json);

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

                default:
                    title = "Save Game Error";
                    icon = MessageBoxImage.Error;
                    break;
            }

            MessageBox.Show(result.Error, title, MessageBoxButton.OK, icon);
        }
    }
}
