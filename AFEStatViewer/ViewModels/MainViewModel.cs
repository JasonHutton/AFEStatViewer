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
        public AchievementsViewModel AFE1HardcoreAchievements { get; }
        public AchievementsViewModel AFE2Achievements { get; }

        public ObservableCollection<AchievementViewModel> AFE1AchievementItems { get; }

        public ObservableCollection<MissionRowViewModel> Missions { get; }
        public ObservableCollection<MissionRowViewModel> GameModeMissions { get; }

        public ObservableCollection<MissionRowViewModel> AFE2Missions { get; }
        public ObservableCollection<MissionRowViewModel> AFE2GameModeMissions { get; }

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

                foreach (var row in AFE2GameModeMissions)
                {
                    row.Progress = value;
                }

                AFE2CampaignCompletion = GetSectionCompletion(AFE2Missions);
                AFE2GameModesCompletion = GetSectionCompletion(AFE2GameModeMissions);
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

            var afe2GameModeRows =
                AFE2GameDefinitions.GameModes
                    .OrderBy(c => c.Number)
                    .SelectMany(c => c.Missions
                        .OrderBy(m => m.Number)
                        .Select(m => new MissionRowViewModel(c, m, AFE2GameDefinitions.ClassKits, AFE2GameDefinitions.ClassKitStyle)))
                    .ToList();

            AFE2GameModeMissions = new ObservableCollection<MissionRowViewModel>(afe2GameModeRows);

            Achievements = new AchievementsViewModel(AFE1GameDefinitions.Achievements);

            AFE1HardcoreAchievements = new AchievementsViewModel(AFE1GameDefinitions.HardcoreAchievements);

            AFE2Achievements = new AchievementsViewModel(AFE2GameDefinitions.Achievements);

            AFE1AchievementItems =
                new ObservableCollection<AchievementViewModel>(
                    Achievements.Items
                        .Concat(AFE1HardcoreAchievements.Items)
                        .OrderBy(
                            achievement => achievement.Name,
                            StringComparer.CurrentCultureIgnoreCase)
                );

            // Achievement totals exist even before a save has been loaded.
            UpdateAFE1AchievementCompletion();

            AFE2AchievementCompletion = CompletionCounter.Count(AFE2Achievements.Items.Select(achievement => achievement.IsComplete));

            // Likewise, section totals can be established before progress is loaded.
            AFE1CampaignCompletion = CompletionCounter.Count(Missions.SelectMany(mission => mission.CompletionStates));
            AFE1GameModesCompletion = CompletionCounter.Count(GameModeMissions.SelectMany(mission => mission.CompletionStates));

            AFE2CampaignCompletion = CompletionCounter.Count(AFE2Missions.SelectMany(mission => mission.CompletionStates));
            AFE2GameModesCompletion = CompletionCounter.Count(AFE2GameModeMissions.SelectMany(mission => mission.CompletionStates));
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

                UpdateAFE1AchievementCompletion();
            }
        }

        public void ApplyAFE1HardcoreJson(string jsonString)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            var ap = _parser.ParseAchievementProgress(
                jsonString,
                AFE1GameDefinitions.HardcoreAchievements
            );

            AFE1HardcoreAchievements.Apply(ap);

            UpdateAFE1AchievementCompletion();
        }

        private void UpdateAFE1AchievementCompletion()
        {
            AFE1AchievementCompletion = CompletionCounter.Count(Achievements.Items.Concat(AFE1HardcoreAchievements.Items).Select(achievement => achievement.IsComplete));
        }

        public void ApplyAFE2Json(string jsonString, bool parseAchievements)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            AFE2Progress = _parser.ParseModeProgress(
                jsonString,
                AFE2GameDefinitions.AllMissions,
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