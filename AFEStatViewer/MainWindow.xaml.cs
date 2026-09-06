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
        private FileSystemWatcher afe1FSW;
        private FileSystemWatcher afe2FSW;
        private Timer afe1SaveGameChangeTimer;
        private Timer afe2SaveGameChangeTimer;
        private readonly object afe1SaveGameChangeLock = new object();
        private readonly object afe2SaveGameChangeLock = new object();
        private bool afe1SaveGameLoadInProgress = false;
        private bool afe2SaveGameLoadInProgress = false;
        private SaveGameLoader afe1SaveGameLoader;
        private SaveGameLoader afe2SaveGameLoader;

        private ViewModels.MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            afe1SaveGameLoader = new SaveGameLoader(
                Environment.ExpandEnvironmentVariables(
                    Properties.Settings.Default.AFE1_SaveGame_Path
                ),
                Properties.Settings.Default.AFE1_SaveGame_Filename
            );

            afe2SaveGameLoader = new SaveGameLoader(
                Environment.ExpandEnvironmentVariables(
                    Properties.Settings.Default.AFE2_SaveGame_Path
                ),
                Properties.Settings.Default.AFE2_SaveGame_Filename
            );
        }

        private FileSystemWatcher CreateSaveGameWatcher(SaveGameLoader saveGameLoader, FileSystemEventHandler changedHandler)
        {
            if (string.IsNullOrEmpty(saveGameLoader.SaveGamePath))
            {
                return null;
            }

            string directory = Path.GetDirectoryName(saveGameLoader.SaveGamePath);
            string filename = Path.GetFileName(saveGameLoader.SaveGamePath);

            FileSystemWatcher fsw = new FileSystemWatcher(directory, filename);

            fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            fsw.Changed += changedHandler;
            fsw.EnableRaisingEvents = true;

            return fsw;
        }

        public void WatchForAFE1SaveGameChanges()
        {
            afe1FSW?.Dispose();
            afe1FSW = CreateSaveGameWatcher(afe1SaveGameLoader, OnAFE1SaveGameChanged);
        }

        public void WatchForAFE2SaveGameChanges()
        {
            afe2FSW?.Dispose();
            afe2FSW = CreateSaveGameWatcher(afe2SaveGameLoader, OnAFE2SaveGameChanged);
        }

        public void OnAFE1SaveGameChanged(object source, FileSystemEventArgs e)
        {
            lock (afe1SaveGameChangeLock)
            {
                afe1SaveGameChangeTimer?.Dispose();

                afe1SaveGameChangeTimer = new Timer(_ => ReloadAFE1SaveGameAfterChange(), null, Properties.Settings.Default.SaveGame_Change_DebounceMS, Timeout.Infinite);
            }
        }

        public void OnAFE2SaveGameChanged(object source, FileSystemEventArgs e)
        {
            lock (afe2SaveGameChangeLock)
            {
                afe2SaveGameChangeTimer?.Dispose();

                afe2SaveGameChangeTimer = new Timer(_ => ReloadAFE2SaveGameAfterChange(), null, Properties.Settings.Default.SaveGame_Change_DebounceMS, Timeout.Infinite);
            }
        }

        private async Task ReloadSaveGameAfterChange(string gameName, SaveGameLoader saveGameLoader, Action<string> applySaveGame)
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
                        applySaveGame(lastResult.Json);
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
                Debug.WriteLine($"{gameName} save game reload failed: {lastResult.Failure}: {lastResult.Error}");
            }
#endif
        }
        
        private async void ReloadAFE1SaveGameAfterChange()
        {
            lock (afe1SaveGameChangeLock)
            {
                if (afe1SaveGameLoadInProgress)
                {
                    return;
                }

                afe1SaveGameLoadInProgress = true;
            }

            try
            {
                await ReloadSaveGameAfterChange("AFE1", afe1SaveGameLoader, ApplyAFE1SaveGame);
            }
            finally
            {
                lock (afe1SaveGameChangeLock)
                {
                    afe1SaveGameLoadInProgress = false;
                }
            }
        }

        private async void ReloadAFE2SaveGameAfterChange()
        {
            lock (afe2SaveGameChangeLock)
            {
                if (afe2SaveGameLoadInProgress)
                {
                    return;
                }

                afe2SaveGameLoadInProgress = true;
            }

            try
            {
                await ReloadSaveGameAfterChange("AFE2", afe2SaveGameLoader, ApplyAFE2SaveGame);
            }
            finally
            {
                lock (afe2SaveGameChangeLock)
                {
                    afe2SaveGameLoadInProgress = false;
                }
            }
        }

        private void ApplyAFE1SaveGame(string jsonString)
        {
            //_vm.ApplyAFE1Json(jsonString, parseAchievements: true);
            _vm.ApplyJson(jsonString, parseAchievements: true);
        }

        private void ApplyAFE2SaveGame(string jsonString)
        {
            //_vm.ApplyAFE2Json(jsonString, parseAchievements: true);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = new ViewModels.MainViewModel(new SaveGameParser());
            DataContext = _vm;

            if (LoadSaveGame("AFE1", afe1SaveGameLoader, ApplyAFE1SaveGame))
            {
                WatchForAFE1SaveGameChanges();
            }

            if (LoadSaveGame("AFE2", afe2SaveGameLoader, ApplyAFE2SaveGame))
            {
                WatchForAFE2SaveGameChanges();
            }
        }

        private bool LoadSaveGame(string gameName, SaveGameLoader saveGameLoader, Action<string> applySaveGame)
        {
            SaveGameLoadResult result = saveGameLoader.LoadSavegame();

            if (!result.Success)
            {
#if DEBUG
                Debug.WriteLine($"{gameName} save game load failed: {result.Failure}: {result.Error}");
#endif
                return false;
            }

            applySaveGame(result.Json);

            return true;
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
