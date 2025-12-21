using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        public static string basePath = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Endeavor\Saved\SaveGames\");
        public static string saveFilename = "char.sav";
        public static List<string> possiblePaths;
        public static string saveGameFinalPath = string.Empty; // Our final result savegame path that we'll be processing.

        private ViewModels.MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Not used/updated currently
        public void WatchForSaveGameChanges()
        {
            fsw = new FileSystemWatcher();
            fsw.Path = System.IO.Path.GetDirectoryName(saveGameFinalPath);
            fsw.Filter = System.IO.Path.GetFileName(saveGameFinalPath);
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Changed += new FileSystemEventHandler(OnSaveGameChanged);
            fsw.EnableRaisingEvents = true;
        }

        public void OnSaveGameChanged(object source, FileSystemEventArgs e)
        {
            LoadSavegame();
        }

        public string FindSaveGame()
        {
            string saveGamePath = string.Empty;

            if (possiblePaths == null || possiblePaths.Count == 0)
            {
                possiblePaths = new List<string>();
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

        public void LoadSavegame()
        {
            if(string.IsNullOrEmpty(saveGameFinalPath))
            {
                saveGameFinalPath = FindSaveGame();
                if (string.IsNullOrEmpty(saveGameFinalPath))
                {
                    MessageBox.Show(string.Format("Save game not found at: {0}", basePath));
                    Application.Current.Shutdown();
                }
            }

            // Read encrypted file bytes once
            byte[] encryptedBytes;
            using (var fs = new FileStream(saveGameFinalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                encryptedBytes = new byte[fs.Length];
                int offset = 0;
                while (offset < encryptedBytes.Length)
                {
                    int read = fs.Read(encryptedBytes, offset, encryptedBytes.Length - offset);
                    if (read == 0) break;
                    offset += read;
                }
            }

            // Try decoders in "most likely newest first" order
            var decoders = new List<ISaveDecoder>
            {
                new Decoders.XOR(0x42, new DecoderOptions(DecoderFlags.SkipLastByte)),
                new Decoders.ShiftModulo(1, 127),
                new Decoders.NOP(),
            };

            var result = SaveGameDecoding.DecodeFirstValidJson(encryptedBytes, decoders);

            if (!result.Success || result.Json == null || result.DecoderUsed == null)
            {
                MessageBox.Show($"Failed to decode save as valid JSON.\n\nLast error:\n{result.Error}");
                return;
            }

            //Debug.WriteLine($"Decoder selected: {result.DecoderUsed.GetType().Name}");

            string jsonString = result.Json;

            //campaignCompletion.LoadCampaignMapData(jsonString);
            //campaignCompletion.LoadPlayerData(jsonString);
            //var parser = new SaveGameParser();

            //var modeProgress = parser.ParseModeProgress(jsonString, GameDefinitions.AllMissions);
            //var achievementProgress = parser.ParseAchievementProgress(jsonString, GameDefinitions.Achievements);

            // TEMP: adapt to existing frontend
            //FrontendAdapter.ApplyToFrontend(campaignCompletion.Frontend, modeProgress, achievementProgress);
            _vm.ApplyJson(jsonString, parseAchievements: true);

        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = new ViewModels.MainViewModel(new SaveGameParser());
            DataContext = _vm;

            LoadSavegame();

            //WatchForSaveGameChanges();
        }
    }
}
