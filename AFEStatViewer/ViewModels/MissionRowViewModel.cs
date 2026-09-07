using AFEStatViewer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

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
        public Brush CompletionSectionBackgroundBrush { get; }
        public Brush CompletionTextBrush { get; }

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

            CompletionSectionBackgroundBrush = CreateFrozenBrush(classKitStyle.CompletionSectionBackgroundColorHex);

            CompletionTextBrush = CreateFrozenBrush(classKitStyle.CompletionTextColorHex);

            ClassKits = new ObservableCollection<ClassKitCompletionViewModel>(
                classKits.Select(classKit =>
                    new ClassKitCompletionViewModel(classKit, mission.SaveKey, classKitStyle))
            );
        }


        private static Brush CreateFrozenBrush(string colorHex)
        {
            var color = (Color)ColorConverter.ConvertFromString(colorHex);
            var brush = new SolidColorBrush(color);
            brush.Freeze();

            return brush;
        }
    }
}