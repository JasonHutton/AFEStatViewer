using AFEStatViewer.Models;

namespace AFEStatViewer.ViewModels
{
    public sealed class AchievementViewModel : ViewModelBase
    {
        public AchievementDefinition Definition { get; }
        public string Name => Definition.Name;
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
                OnPropertyChanged(nameof(Display));
            }
        }

        public bool IsComplete => Value >= Target;
        public string Display => $"{Name}: {Value} / {Target}";

        public AchievementViewModel(AchievementDefinition def) => Definition = def;
    }
}
