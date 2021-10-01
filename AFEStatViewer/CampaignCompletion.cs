using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;

using NAryDictionary;

namespace AFEStatViewer
{
    public class CampaignCompletion
    {
        public NAryDictionary<string, string, int> data { get; set; }
        private string[] difficultyKeys = { "Easy|Campaign", "Normal|Campaign", "Hard|Campaign", "Extreme|Campaign", "Insane|Campaign" };
        public CampaignCompletionFrontend Frontend { get; set; }

        public CampaignCompletion()
        {
            data = new NAryDictionary<string, string, int>();
            Frontend = new CampaignCompletionFrontend();

            // Add all the maps
            AddMap("Campaign|SC-C1|SC-C1M1");
            //AddMap("Campaign|SC-C1|SC-C1M2");
            AddMap("Campaign|SC-C1|SC_C1M2"); // This appears to be a typo in the original game data.
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
                    if (sets.TryGetProperty(difficulty, out JsonElement campaignDifficulty))
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

            foreach (string mapKey in data.Keys)
            {
                foreach (string difficulty in difficultyKeys)
                {
                    if (data[mapKey][difficulty] > 0)
                        Frontend.Set(mapKey, difficulty, true);
                    else
                        Frontend.Set(mapKey, difficulty, false);
                }
            }

                    //UpdateFrontend();
        }
        /*
        void UpdateFrontend()
        {
            if (data["Campaign|SC-C1|SC-C1M1"]["Easy|Campaign"] > 0)
                Frontend.Casual_C1M1 = true;
            else
                Frontend.Casual_C1M1 = false;

            if (data["Campaign|SC-C1|SC-C1M1"]["Normal|Campaign"] > 0)
                Frontend.Standard_C1M1 = true;
            else
                Frontend.Standard_C1M1 = false;

            if (data["Campaign|SC-C1|SC-C1M1"]["Hard|Campaign"] > 0)
                Frontend.Intense_C1M1 = true;
            else
                Frontend.Intense_C1M1 = false;

            if (data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"] > 0)
                Frontend.Insane_C1M1 = true;
            else
                Frontend.Insane_C1M1 = false;

            if (data["Campaign|SC-C1|SC-C1M2"]["Insane|Campaign"] > 0)
                Frontend.Insane_C1M2 = true;
            else
                Frontend.Insane_C1M2 = false;

            if (data["Campaign|SC-C1|SC-C1M3"]["Insane|Campaign"] > 0)
                Frontend.Insane_C1M3 = true;
            else
                Frontend.Insane_C1M3 = false;
        }*/
    }
}
