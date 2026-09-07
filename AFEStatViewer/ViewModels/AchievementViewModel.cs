using AFEStatViewer.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AFEStatViewer.ViewModels
{
    public sealed class AchievementViewModel : ViewModelBase
    {
        public AchievementDefinition Definition { get; }
        public string Id => Definition.Id;
        public string Name => Definition.Name;
        public string Description => Definition.Description;
        public int Target => Definition.Target;

        private readonly ImageSource? _incompleteIcon;
        private readonly ImageSource? _completeIcon;

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
                OnPropertyChanged(nameof(Icon));
                OnPropertyChanged(nameof(IconOpacity));
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

        public AchievementViewModel(AchievementDefinition def)
        {
            Definition = def;

            _incompleteIcon = LoadIcon($"{Id}_Incomplete.png");
            _completeIcon = LoadIcon($"{Id}_Complete.png");
        }

        private static ImageSource? LoadIcon(string filename)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/Resources/Achievements/{filename}", UriKind.Absolute);

                var resource = Application.GetResourceStream(uri);

                if (resource == null)
                {
                    return null;
                }

                using (resource.Stream)
                {
                    var image = new BitmapImage();

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = resource.Stream;
                    image.EndInit();
                    image.Freeze();

                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        public ImageSource? Icon
        {
            get
            {
                if (IsComplete)
                {
                    return _completeIcon;
                }

                return _incompleteIcon ?? _completeIcon;
            }
        }

        public double IconOpacity
        {
            get
            {
                if (IsComplete)
                {
                    return 1.0;
                }

                return _incompleteIcon != null ? 1.0 : 0.2;
            }
        }
    }
}