using System;
using System.Collections.Generic;
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

using System.IO;
using System.Diagnostics;

using NAryDictionary;

namespace AFEStatViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemWatcher fsw;
        public static string inputPath = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Endeavor\Saved\SaveGames\char.sav");
        private CampaignCompletion campaignCompletion;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void WatchForSaveGameChanges()
        {
            fsw = new FileSystemWatcher();
            fsw.Path = System.IO.Path.GetDirectoryName(inputPath);
            fsw.Filter = System.IO.Path.GetFileName(inputPath);
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Changed += new FileSystemEventHandler(OnSaveGameChanged);
            fsw.EnableRaisingEvents = true;
        }

        public void OnSaveGameChanged(object source, FileSystemEventArgs e)
        {
            LoadSavegame();
        }

        public void LoadSavegame()
        {
            if (!File.Exists(inputPath))
            {
                MessageBox.Show(string.Format("Save game not found at: {0}", inputPath));
                return;
            }

            StringBuilder sb = new StringBuilder();
            using (BinaryReader reader = new BinaryReader(File.OpenRead(inputPath)))
            {
                int bytesPerRead = 20;
                byte[] byteArray;
                do
                {
                    byteArray = reader.ReadBytes(bytesPerRead);

                    for (int i = 0; i < byteArray.Count(); i++)
                    {
                        // Offset the byte's value by 1, and use modulo to wrap if it's more than an allowed value.
                        byteArray[i] = (byte)((byteArray[i] + 1) % 127);
                    }

                    // Add the bytes into our output string
                    sb.Append(Encoding.ASCII.GetString(byteArray));
                }
                while (byteArray.Count() > 0);
                reader.Close();
            }

            string jsonString = sb.ToString();

            campaignCompletion.LoadCampaignMapData(jsonString);
            campaignCompletion.LoadPlayerData(jsonString);
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            campaignCompletion = new CampaignCompletion();
            DataContext = campaignCompletion.Frontend;

            LoadSavegame();

            //WatchForSaveGameChanges();
        }
    }
}
