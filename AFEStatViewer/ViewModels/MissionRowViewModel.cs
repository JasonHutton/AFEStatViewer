using AFEStatViewer.Models;
using System.Collections.Generic;

namespace AFEStatViewer.ViewModels
{
    public sealed class MissionRowViewModel : ViewModelBase
    {
        public MissionDefinition Mission { get; }

        public int CampaignNumber { get; }
        public string CampaignName { get; }

        public int MissionNumber => Mission.Number;
        public string MissionName => Mission.Name;
        public string SaveKey => Mission.SaveKey;
        public string MissionCode => $"{CampaignNumber}-{MissionNumber}";

        private ModeProgress? _progress;
        public ModeProgress? Progress
        {
            get => _progress;
            set
            {
                if (!SetProperty(ref _progress, value)) return;
                OnPropertyChanged(nameof(CasualCompleted));
                OnPropertyChanged(nameof(StandardCompleted));
                OnPropertyChanged(nameof(IntenseCompleted));
                OnPropertyChanged(nameof(ExtremeCompleted));
                OnPropertyChanged(nameof(InsaneCompleted));
            }
        }

        public bool CasualCompleted => Progress?.GetCount(SaveKey, Difficulty.Casual) > 0;
        public bool StandardCompleted => Progress?.GetCount(SaveKey, Difficulty.Standard) > 0;
        public bool IntenseCompleted => Progress?.GetCount(SaveKey, Difficulty.Intense) > 0;
        public bool ExtremeCompleted => Progress?.GetCount(SaveKey, Difficulty.Extreme) > 0;
        public bool InsaneCompleted => Progress?.GetCount(SaveKey, Difficulty.Insane) > 0;

        public MissionRowViewModel(CampaignDefinition campaign, MissionDefinition mission)
        {
            CampaignNumber = campaign.Number;
            CampaignName = campaign.Name;
            Mission = mission;
        }
    }
}
