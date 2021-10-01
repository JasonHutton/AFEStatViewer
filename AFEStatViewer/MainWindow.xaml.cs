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
using System.Text.Json;
using System.Diagnostics;

namespace AFEStatViewer
{
    public class CampaignCompletion
    {
        public CampaignCompletion()
        {
            data = new NAryDictionary<string, string, int>();

            // Add all the maps
            AddMap("Campaign|SC-C1|SC-C1M1");
            AddMap("Campaign|SC-C1|SC-C1M2");
            AddMap("Campaign|SC-C1|SC-C1M3");

            AddMap("Campaign|SC-C2|SC-C2M1");
            AddMap("Campaign|SC-C2|SC-C2M2");
            AddMap("Campaign|SC-C2|SC-C2M3");

            AddMap("Campaign|SC-C3|SC-C3M1");
            AddMap("Campaign|SC-C3|SC-C3M2");
            AddMap("Campaign|SC-C3|SC-C3M3");

            AddMap("Campaign|SC-C4|SC-C4M1");
            AddMap("Campaign|SC-C4|SC-C4M2");
            AddMap("Campaign|SC-C4|SC-C4M3");
        }

        /// <summary>
        /// Add all the difficulties for this map
        /// </summary>
        /// <param name="map"></param>
        public void AddMap(string map)
        {
            data[map] = data.New();

            foreach (string difficulty in difficultyKeys)
            {
                data[map][difficulty] = 0;
            }
        }

        /// <summary>
        /// Parse map and difficulty completion for all maps and difficulties
        /// </summary>
        /// <param name="jsonString"></param>
        public void LoadCampaignMapData(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                JsonElement counterTrackerElement = root.GetProperty("CounterTracker");
                JsonElement sets = counterTrackerElement.GetProperty("Sets");

                foreach (string difficulty in difficultyKeys)
                {
                    // Parse the difficulty element
                    if(sets.TryGetProperty(difficulty, out JsonElement campaignDifficulty))
                    {
                        JsonElement vars = campaignDifficulty.GetProperty("Vars");

                        foreach (string mapKey in data.Keys)
                        {
                            // Parse the map element
                            if (vars.TryGetProperty(mapKey, out JsonElement mapElement))
                            {
                                data[mapKey][difficulty] = mapElement.GetInt32(); ;
                            }
                        }
                    }
                }
            }
        }

        public NAryDictionary<string, string, int> data { get; set; }
        private string[] difficultyKeys = { "Easy|Campaign" , "Normal|Campaign", "Hard|Campaign" , "Extreme|Campaign" , "Insane|Campaign" };
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string inputPath = @"..\..\..\..\char29.sav";

            BinaryReader reader = new BinaryReader(File.OpenRead(inputPath));

            StringBuilder sb = new StringBuilder();
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
            string jsonString = sb.ToString();

            var campaignCompletion = new CampaignCompletion();

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"]);

            campaignCompletion.LoadCampaignMapData(jsonString);

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"]);

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M3"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M3"]["Easy|Campaign"]);
        }
    }
}
