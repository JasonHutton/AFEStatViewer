using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AFEStatViewer
{
    public class CampaignCompletionFrontend : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Campaign 1, all difficulties
        public bool Casual_C1M1 { get; set; }
        public bool Casual_C1M2 { get; set; }
        public bool Casual_C1M3 { get; set; }

        public bool Standard_C1M1 { get; set; }
        public bool Standard_C1M2 { get; set; }
        public bool Standard_C1M3 { get; set; }

        public bool Intense_C1M1 { get; set; }
        public bool Intense_C1M2 { get; set; }
        public bool Intense_C1M3 { get; set; }

        public bool Extreme_C1M1 { get; set; }
        public bool Extreme_C1M2 { get; set; }
        public bool Extreme_C1M3 { get; set; }

        public bool Insane_C1M1 { get; set; }
        public bool Insane_C1M2 { get; set; }
        public bool Insane_C1M3 { get; set; }

        // Campaign 2, all difficulties
        public bool Casual_C2M1 { get; set; }
        public bool Casual_C2M2 { get; set; }
        public bool Casual_C2M3 { get; set; }

        public bool Standard_C2M1 { get; set; }
        public bool Standard_C2M2 { get; set; }
        public bool Standard_C2M3 { get; set; }

        public bool Intense_C2M1 { get; set; }
        public bool Intense_C2M2 { get; set; }
        public bool Intense_C2M3 { get; set; }

        public bool Extreme_C2M1 { get; set; }
        public bool Extreme_C2M2 { get; set; }
        public bool Extreme_C2M3 { get; set; }

        public bool Insane_C2M1 { get; set; }
        public bool Insane_C2M2 { get; set; }
        public bool Insane_C2M3 { get; set; }

        // Campaign 3, all difficulties
        public bool Casual_C3M1 { get; set; }
        public bool Casual_C3M2 { get; set; }
        public bool Casual_C3M3 { get; set; }

        public bool Standard_C3M1 { get; set; }
        public bool Standard_C3M2 { get; set; }
        public bool Standard_C3M3 { get; set; }

        public bool Intense_C3M1 { get; set; }
        public bool Intense_C3M2 { get; set; }
        public bool Intense_C3M3 { get; set; }

        public bool Extreme_C3M1 { get; set; }
        public bool Extreme_C3M2 { get; set; }
        public bool Extreme_C3M3 { get; set; }

        public bool Insane_C3M1 { get; set; }
        public bool Insane_C3M2 { get; set; }
        public bool Insane_C3M3 { get; set; }

        // Campaign 4, all difficulties
        public bool Casual_C4M1 { get; set; }
        public bool Casual_C4M2 { get; set; }
        public bool Casual_C4M3 { get; set; }

        public bool Standard_C4M1 { get; set; }
        public bool Standard_C4M2 { get; set; }
        public bool Standard_C4M3 { get; set; }

        public bool Intense_C4M1 { get; set; }
        public bool Intense_C4M2 { get; set; }
        public bool Intense_C4M3 { get; set; }

        public bool Extreme_C4M1 { get; set; }
        public bool Extreme_C4M2 { get; set; }
        public bool Extreme_C4M3 { get; set; }

        public bool Insane_C4M1 { get; set; }
        public bool Insane_C4M2 { get; set; }
        public bool Insane_C4M3 { get; set; }

        // Achievements
        public AchievementCount AchHighVoltage { get; set; }
        public AchievementCount AchIThinkTheyLikeMe { get; set; }

        // There's probably some better way to access all these properties without so much typing, but I don't know it offhand.
        public void Set(string map, string difficulty, bool value)
        {
            switch (map)
            {
                case "Campaign|SC-C1|SC-C1M1":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C1M1 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C1M1 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C1M1 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C1M1 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C1M1 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C1|SC_C1M2": // This appears to be a typo in the original game data.
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C1M2 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C1M2 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C1M2 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C1M2 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C1M2 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C1|SC-C1M3":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C1M3 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C1M3 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C1M3 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C1M3 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C1M3 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C2|SC-C2M1":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C2M1 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C2M1 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C2M1 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C2M1 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C2M1 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C2|SC-C2M2":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C2M2 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C2M2 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C2M2 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C2M2 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C2M2 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C2|SC-C2M3":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C2M3 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C2M3 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C2M3 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C2M3 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C2M3 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C3|SC-C3M1":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C3M1 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C3M1 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C3M1 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C3M1 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C3M1 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C3|SC-C3M2":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C3M2 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C3M2 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C3M2 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C3M2 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C3M2 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C3|SC-C3M3":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C3M3 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C3M3 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C3M3 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C3M3 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C3M3 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C4|SC-C4M1":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C4M1 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C4M1 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C4M1 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C4M1 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C4M1 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C4|SC-C4M2":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C4M2 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C4M2 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C4M2 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C4M2 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C4M2 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Campaign|SC-C4|SC-C4M3":
                    switch (difficulty)
                    {
                        case "Easy|Campaign":
                            Casual_C4M3 = value;
                            break;
                        case "Normal|Campaign":
                            Standard_C4M3 = value;
                            break;
                        case "Hard|Campaign":
                            Intense_C4M3 = value;
                            break;
                        case "Extreme|Campaign":
                            Extreme_C4M3 = value;
                            break;
                        case "Insane|Campaign":
                            Insane_C4M3 = value;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;

            }
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
