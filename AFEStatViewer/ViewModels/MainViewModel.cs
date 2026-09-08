using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
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

        private CompletionSummary _afe1CampaignCompletion = new(0, 0);
        public CompletionSummary AFE1CampaignCompletion
        {
            get => _afe1CampaignCompletion;
            private set
            {
                if (!SetProperty(ref _afe1CampaignCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE1CampaignCompletionHeader));
            }
        }

        private CompletionSummary _afe1GameModesCompletion = new(0, 0);
        public CompletionSummary AFE1GameModesCompletion
        {
            get => _afe1GameModesCompletion;
            private set
            {
                if (!SetProperty(ref _afe1GameModesCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE1GameModesCompletionHeader));
            }
        }

        private CompletionSummary _afe1AchievementCompletion = new(0, 0);
        public CompletionSummary AFE1AchievementCompletion
        {
            get => _afe1AchievementCompletion;
            private set
            {
                if (!SetProperty(ref _afe1AchievementCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE1AchievementsHeader));
            }
        }

        private CompletionSummary _afe2CampaignCompletion = new(0, 0);
        public CompletionSummary AFE2CampaignCompletion
        {
            get => _afe2CampaignCompletion;
            private set
            {
                if (!SetProperty(ref _afe2CampaignCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE2CampaignCompletionHeader));
            }
        }

        private CompletionSummary _afe2GameModesCompletion = new(0, 0);
        public CompletionSummary AFE2GameModesCompletion
        {
            get => _afe2GameModesCompletion;
            private set
            {
                if (!SetProperty(ref _afe2GameModesCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE2GameModesCompletionHeader));
            }
        }

        private CompletionSummary _afe2AchievementCompletion = new(0, 0);
        public CompletionSummary AFE2AchievementCompletion
        {
            get => _afe2AchievementCompletion;
            private set
            {
                if (!SetProperty(ref _afe2AchievementCompletion, value))
                    return;

                OnPropertyChanged(nameof(AFE2AchievementsHeader));
            }
        }

        public string AFE1CampaignCompletionHeader =>
            $"Campaign Completion ({AFE1CampaignCompletion.Completed}/{AFE1CampaignCompletion.Total})";

        public string AFE1GameModesCompletionHeader =>
            $"Game Modes Completion ({AFE1GameModesCompletion.Completed}/{AFE1GameModesCompletion.Total})";

        public string AFE1AchievementsHeader =>
            $"Achievements ({AFE1AchievementCompletion.Completed}/{AFE1AchievementCompletion.Total})";

        public string AFE2CampaignCompletionHeader =>
            $"Campaign Completion ({AFE2CampaignCompletion.Completed}/{AFE2CampaignCompletion.Total})";

        public string AFE2GameModesCompletionHeader =>
            $"Game Modes Completion ({AFE2GameModesCompletion.Completed}/{AFE2GameModesCompletion.Total})";

        public string AFE2AchievementsHeader =>
            $"Achievements ({AFE2AchievementCompletion.Completed}/{AFE2AchievementCompletion.Total})";

        private ModeProgress? _progress;
        public ModeProgress? Progress
        {
            get => _progress;
            private set
            {
                if (!SetProperty(ref _progress, value))
                    return;

                foreach (var row in Missions)
                {
                    row.Progress = value;
                }

                foreach (var row in GameModeMissions)
                {
                    row.Progress = value;
                }

                AFE1CampaignCompletion = GetSectionCompletion(Missions);
                AFE1GameModesCompletion = GetSectionCompletion(GameModeMissions);
            }
        }

        private ModeProgress? _afe2Progress;
        public ModeProgress? AFE2Progress
        {
            get => _afe2Progress;
            private set
            {
                if (!SetProperty(ref _afe2Progress, value))
                    return;

                foreach (var row in AFE2Missions)
                {
                    row.Progress = value;
                }

                AFE2CampaignCompletion = GetSectionCompletion(AFE2Missions);
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
                        .Select(m => new MissionRowViewModel(
                            c,
                            m,
                            AFE1GameDefinitions.ClassKits,
                            AFE1GameDefinitions.ClassKitStyle)))
                    .ToList();

            Missions = new ObservableCollection<MissionRowViewModel>(campaignRows);

            var gameModeRows =
                AFE1GameDefinitions.GameModes
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(
                            c,
                            m,
                            AFE1GameDefinitions.ClassKits,
                            AFE1GameDefinitions.ClassKitStyle)))
                    .ToList();

            GameModeMissions = new ObservableCollection<MissionRowViewModel>(gameModeRows);

            var afe2CampaignRows =
                AFE2GameDefinitions.Campaigns
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(
                            c,
                            m,
                            AFE2GameDefinitions.ClassKits,
                            AFE2GameDefinitions.ClassKitStyle)))
                    .ToList();

            AFE2Missions = new ObservableCollection<MissionRowViewModel>(afe2CampaignRows);

            Achievements = new AchievementsViewModel(AFE1GameDefinitions.Achievements);
            AFE2Achievements = new AchievementsViewModel(AFE2GameDefinitions.Achievements);

            // AFE2 game modes are not implemented in the UI yet.
            AFE2GameModesCompletion = new CompletionSummary(0, 0);

            // Achievement totals exist even before a save has been loaded.
            AFE1AchievementCompletion = CompletionCounter.Count(Achievements.Items.Select(achievement => achievement.IsComplete));
            AFE2AchievementCompletion = CompletionCounter.Count(AFE2Achievements.Items.Select(achievement => achievement.IsComplete));

            // Likewise, section totals can be established before progress is loaded.
            AFE1CampaignCompletion = CompletionCounter.Count(Missions.SelectMany(mission => mission.CompletionStates));
            AFE1GameModesCompletion = CompletionCounter.Count(GameModeMissions.SelectMany(mission => mission.CompletionStates));

            AFE2CampaignCompletion = CompletionCounter.Count(AFE2Missions.SelectMany(mission => mission.CompletionStates));
        }

        public void ApplyAFE1Json(string jsonString, bool parseAchievements)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            Progress = _parser.ParseModeProgress(
                jsonString,
                AFE1GameDefinitions.AllMissions,
                AFE1GameDefinitions.ClassKits
            );

            if (parseAchievements)
            {
                var ap = _parser.ParseAchievementProgress(
                    jsonString,
                    AFE1GameDefinitions.Achievements
                );

                Achievements.Apply(ap);

                AFE1AchievementCompletion = CompletionCounter.Count(Achievements.Items.Select(achievement => achievement.IsComplete));
            }
        }

        public void ApplyAFE2Json(string jsonString, bool parseAchievements)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            AFE2Progress = _parser.ParseModeProgress(
                jsonString,
                AFE2GameDefinitions.CampaignOnlyMissions,
                AFE2GameDefinitions.ClassKits
            );

            if (parseAchievements)
            {
                var ap = _parser.ParseAchievementProgress(
                    jsonString,
                    AFE2GameDefinitions.Achievements
                );

                AFE2Achievements.Apply(ap);

                AFE2AchievementCompletion = CompletionCounter.Count(AFE2Achievements.Items.Select(achievement => achievement.IsComplete));
            }
        }

        private static CompletionSummary GetSectionCompletion(IEnumerable<MissionRowViewModel> rows)
        {
            return CompletionCounter.Count(rows.SelectMany(row => row.CompletionStates));
        }
    }
}