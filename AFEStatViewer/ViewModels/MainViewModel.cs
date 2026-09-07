using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AFEStatViewer.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly SaveGameParser _parser;

        public AchievementsViewModel Achievements { get; }
        public AchievementsViewModel AFE2Achievements { get; }

        public ObservableCollection<MissionRowViewModel> Missions { get; }
        public ObservableCollection<MissionRowViewModel> GameModeMissions { get; }

        public ObservableCollection<MissionRowViewModel> AFE2Missions { get; }

        private ModeProgress? _progress;
        public ModeProgress? Progress
        {
            get => _progress;
            private set
            {
                if (!SetProperty(ref _progress, value)) return;

                foreach (var row in Missions)
                    row.Progress = value;

                foreach (var row in GameModeMissions)
                    row.Progress = value;
            }
        }

        private ModeProgress? _afe2Progress;
        public ModeProgress? AFE2Progress
        {
            get => _afe2Progress;
            private set
            {
                if (!SetProperty(ref _afe2Progress, value)) return;

                foreach (var row in AFE2Missions)
                    row.Progress = value;
            }
        }

        public MainViewModel(SaveGameParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            var campaignRows =
                AFE1GameDefinitions.Campaigns
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m, AFE1GameDefinitions.ClassKits)))
                    .ToList();

            Missions = new ObservableCollection<MissionRowViewModel>(campaignRows);

            var gameModeRows =
                AFE1GameDefinitions.GameModes
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m, AFE1GameDefinitions.ClassKits)))
                    .ToList();

            GameModeMissions = new ObservableCollection<MissionRowViewModel>(gameModeRows);

            var afe2CampaignRows =
                AFE2GameDefinitions.Campaigns
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m, AFE2GameDefinitions.ClassKits)))
                    .ToList();

            AFE2Missions = new ObservableCollection<MissionRowViewModel>(afe2CampaignRows);

            Achievements = new AchievementsViewModel(AFE1GameDefinitions.Achievements);
            AFE2Achievements = new AchievementsViewModel(AFE2GameDefinitions.Achievements);
        }

        public void ApplyAFE1Json(string jsonString, bool parseAchievements)
        {
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));

            Progress = _parser.ParseModeProgress(jsonString, AFE1GameDefinitions.AllMissions, AFE1GameDefinitions.ClassKits);

            if (parseAchievements)
            {
                var ap = _parser.ParseAchievementProgress(jsonString, AFE1GameDefinitions.Achievements);
                Achievements.Apply(ap);
            }
        }

        public void ApplyAFE2Json(string jsonString, bool parseAchievements)
        {
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));

            AFE2Progress = _parser.ParseModeProgress(jsonString, AFE2GameDefinitions.CampaignOnlyMissions, AFE2GameDefinitions.ClassKits);

            if (parseAchievements)
            {
                var ap = _parser.ParseAchievementProgress(jsonString, AFE2GameDefinitions.Achievements);
                AFE2Achievements.Apply(ap);
            }
        }
    }
}