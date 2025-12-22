using AFEStatViewer.Models;

namespace AFEStatViewer.ViewModels
{
    public sealed class AchievementViewModel : ViewModelBase
    {
        public AchievementDefinition Definition { get; }
        public string Name => Definition.Name;
        public string Description => Definition.Description;
        public int Target => Definition.Target;
        public string Key => Definition.Key;

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (!SetProperty(ref _value, value)) return;

                OnPropertyChanged(nameof(IsComplete));
                OnPropertyChanged(nameof(PercentComplete));
                OnPropertyChanged(nameof(ProgressDisplay));
            }
        }

        public bool IsComplete => Target > 0 && Value >= Target;

        /// <summary>
        /// Percentage in range [0, 100]. Bind with StringFormat in XAML.
        /// </summary>
        public double PercentComplete
        {
            get
            {
                if (Target <= 0) return 0;

                double percent = (double)Value / Target * 100.0;
                if (percent < 0) return 0;
                if (percent > 100) return 100;
                return percent;
            }
        }
        /// <summary>
        /// Format: "(progress/target)"
        /// </summary>
        public string ProgressDisplay => $"({Value}/{Target})";

        public AchievementViewModel(AchievementDefinition def) => Definition = def;
    }
}
