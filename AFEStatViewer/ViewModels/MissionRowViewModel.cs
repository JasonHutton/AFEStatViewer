using AFEStatViewer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public string CampaignMissionName => $"{CampaignName}: {MissionName}";

        public ObservableCollection<ClassKitCompletionViewModel> ClassKits { get; }

        private ModeProgress? _progress;
        public ModeProgress? Progress
        {
            get => _progress;
            set
            {
                if (!SetProperty(ref _progress, value)) return;

                foreach (var classKit in ClassKits)
                {
                    classKit.Progress = value;
                }
            }
        }

        public MissionRowViewModel(
            CampaignDefinition campaign,
            MissionDefinition mission,
            IEnumerable<ClassKitDefinition> classKits,
            ClassKitVisualStyle classKitStyle)
        {
            CampaignNumber = campaign.Number;
            CampaignName = campaign.Name;
            Mission = mission;

            ClassKits = new ObservableCollection<ClassKitCompletionViewModel>(
                classKits.Select(classKit =>
                    new ClassKitCompletionViewModel(classKit, mission.SaveKey, classKitStyle))
            );
        }
    }
}