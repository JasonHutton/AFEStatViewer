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

        public string FindSaveGame()
        {
            string saveGamePath = string.Empty;
            List<string> possiblePaths = new List<string>();

            if (!Directory.Exists(basePath))
            {
                return string.Empty;
            }

            // Timestamp gets updated when you load game to menu, then close the game.
            possiblePaths.Add(basePath); // We want to check the basepath (Season 1 and earlier AFE behavior.)
            possiblePaths.AddRange(Directory.GetDirectories(basePath)); // And we also want to check subdirectories within that. (Season 2 AFE behavior.)

            // If a path doesn't exist, discard it
            for (int i = 0; i < possiblePaths.Count; i++)
            {
                possiblePaths[i] = System.IO.Path.Combine(possiblePaths[i], saveFilename);
                if (!File.Exists(possiblePaths[i]))
                {
                    possiblePaths.RemoveAt(i);
                    i--; // Decrement counter so that the end of loop increment indexes the correct element, as one has been removed.
                }
            }

            // Find the most recently accessed savegame from our paths.
            DateTime mostRecentFileTime = DateTime.MinValue;
            for (int i = 0; i < possiblePaths.Count; i++)
            {
                DateTime lastAccessedTime = File.GetLastWriteTimeUtc(possiblePaths[i]);
                if (lastAccessedTime > mostRecentFileTime)
                {
                    mostRecentFileTime = lastAccessedTime;
                    saveGamePath = possiblePaths[i];
                }
            }

            return saveGamePath;
        }

        private string GetSaveGamePath()
        {
            if (!string.IsNullOrEmpty(saveGameFinalPath))
            {
                return saveGameFinalPath;
            }

            saveGameFinalPath = FindSaveGame();

            return saveGameFinalPath;
        }

        private byte[] ReadSaveGame(string saveGamePath)
        {
            using (var fs = new FileStream(saveGamePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] encryptedBytes = new byte[fs.Length];

                int offset = 0;
                while (offset < encryptedBytes.Length)
                {
                    int read = fs.Read(encryptedBytes, offset, encryptedBytes.Length - offset);
                    if (read == 0) break;
                    offset += read;
                }

                if (offset != encryptedBytes.Length)
                {
                    throw new IOException($"Expected to read {encryptedBytes.Length} bytes, but only read {offset}.");
                }

                return encryptedBytes;
            }
        }

        private DecodeAttemptResult DecodeSaveGame(byte[] encryptedBytes)
        {
            // Try decoders in "most likely newest first" order
            var decoders = new List<ISaveDecoder>
            {
                new Decoders.XOR(0x42, new DecoderOptions(DecoderFlags.SkipLastByte)),
                new Decoders.ShiftModulo(1, 127),
                new Decoders.NOP(),
            };

            return SaveGameDecoding.DecodeFirstValidJson(encryptedBytes, decoders);
        }

        private void ApplySaveGame(string jsonString)
        {
            _vm.ApplyJson(jsonString, parseAchievements: true);
        }

        public SaveGameLoadResult LoadSavegame()
        {
            string saveGamePath = GetSaveGamePath();

            if (string.IsNullOrEmpty(saveGamePath))
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.SaveGameNotFound, $"Save game not found at: {basePath}");
            }

            byte[] encryptedBytes;

            try
            {
                encryptedBytes = ReadSaveGame(saveGamePath);
            }
            catch (IOException ex)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.ReadFailed, ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.ReadFailed, ex.Message, ex);
            }

            DecodeAttemptResult decodeResult = DecodeSaveGame(encryptedBytes);

            if (!decodeResult.Success || decodeResult.Json == null || decodeResult.DecoderUsed == null)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.DecodeFailed, decodeResult.Error);
            }

#if DEBUG
            Debug.WriteLine($"Decoder selected: {decodeResult.DecoderUsed.GetType().Name}");
#endif

#if DEBUG && SAVE_JSON
            string outputPath = System.IO.Path.Combine(AppContext.BaseDirectory, Properties.Settings.Default.AFE1_SaveGame_Output_Filename);

            File.WriteAllText(outputPath, decodeResult.Json, Encoding.UTF8);
#endif

            ApplySaveGame(decodeResult.Json);

            return SaveGameLoadResult.Successful();
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
