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

        public ObservableCollection<MissionRowViewModel> Missions { get; }
        public ObservableCollection<MissionRowViewModel> GameModeMissions { get; }

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

        public MainViewModel(SaveGameParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));

            var campaignRows =
                GameDefinitions.Campaigns
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m)))
                    .ToList();

            Missions = new ObservableCollection<MissionRowViewModel>(campaignRows);

            var gameModeRows =
                GameDefinitions.GameModes
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m)))
                    .ToList();

            GameModeMissions = new ObservableCollection<MissionRowViewModel>(gameModeRows);

            Achievements = new AchievementsViewModel(GameDefinitions.Achievements);
        }

        public void ApplyJson(string jsonString, bool parseAchievements)
        {
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));

            Progress = _parser.ParseModeProgress(jsonString, GameDefinitions.AllMissions);

            if (parseAchievements)
            {
                var ap = _parser.ParseAchievementProgress(jsonString, GameDefinitions.Achievements);
                Achievements.Apply(ap);
            }
        }
    }
}
