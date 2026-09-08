using AFEStatViewer.Models;
using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace AFEStatViewer.ViewModels
{
    public sealed class ClassKitCompletionViewModel : ViewModelBase
    {
        public ClassKitDefinition ClassKit { get; }

        public string Name => ClassKit.Name;
        public string SaveKey => ClassKit.SaveKey;
        public string MissionSaveKey { get; }

        public ImageSource? IconMask { get; }
        public bool HasIcon => IconMask != null;
        public double IconSize { get; }

        private readonly Brush _completeBrush;
        private readonly Brush _incompleteBrush;
        private readonly Brush _iconBackgroundBrush;
        private readonly double _completeOpacity;
        private readonly double _incompleteOpacity;
        private readonly double _iconBackgroundOpacity;

        private ModeProgress? _progress;
        public ModeProgress? Progress
        {
            get => _progress;
            set
            {
                if (!SetProperty(ref _progress, value)) return;

                RaiseDifficultyProperties();
            }
        }

        public bool CasualCompleted => IsCompleted(Difficulty.Casual);
        public bool StandardCompleted => IsCompleted(Difficulty.Standard);
        public bool IntenseCompleted => IsCompleted(Difficulty.Intense);
        public bool ExtremeCompleted => IsCompleted(Difficulty.Extreme);
        public bool InsaneCompleted => IsCompleted(Difficulty.Insane);

        public IEnumerable<bool> CompletionStates
        {
            get
            {
                yield return CasualCompleted;
                yield return StandardCompleted;
                yield return IntenseCompleted;
                yield return ExtremeCompleted;
                yield return InsaneCompleted;
            }
        }

        public Brush CasualIconBrush => GetIconBrush(Difficulty.Casual);
        public Brush StandardIconBrush => GetIconBrush(Difficulty.Standard);
        public Brush IntenseIconBrush => GetIconBrush(Difficulty.Intense);
        public Brush ExtremeIconBrush => GetIconBrush(Difficulty.Extreme);
        public Brush InsaneIconBrush => GetIconBrush(Difficulty.Insane);

        public double CasualIconOpacity => GetIconOpacity(Difficulty.Casual);
        public double StandardIconOpacity => GetIconOpacity(Difficulty.Standard);
        public double IntenseIconOpacity => GetIconOpacity(Difficulty.Intense);
        public double ExtremeIconOpacity => GetIconOpacity(Difficulty.Extreme);
        public double InsaneIconOpacity => GetIconOpacity(Difficulty.Insane);
        public Brush IconBackgroundBrush => _iconBackgroundBrush;
        public double IconBackgroundOpacity => _iconBackgroundOpacity;

        public ClassKitCompletionViewModel(
            ClassKitDefinition classKit,
            string missionSaveKey,
            ClassKitVisualStyle visualStyle)
        {
            ClassKit = classKit ?? throw new ArgumentNullException(nameof(classKit));
            MissionSaveKey = missionSaveKey ?? throw new ArgumentNullException(nameof(missionSaveKey));

            IconMask = ResourceImageCache.LoadClassIcon(classKit.IconResourceName);
            IconSize = visualStyle.IconSize;

            _completeBrush = CreateFrozenBrush(visualStyle.CompleteColorHex);
            _incompleteBrush = CreateFrozenBrush(visualStyle.IncompleteColorHex);
            _iconBackgroundBrush = CreateFrozenBrush(visualStyle.IconBackgroundColorHex);

            _completeOpacity = visualStyle.CompleteOpacity;
            _incompleteOpacity = visualStyle.IncompleteOpacity;
            _iconBackgroundOpacity = visualStyle.IconBackgroundOpacity;
        }

        private bool IsCompleted(Difficulty difficulty)
        {
            return Progress?.GetCount(MissionSaveKey, difficulty, ClassKit.SaveKey) > 0;
        }

        private Brush GetIconBrush(Difficulty difficulty)
        {
            return IsCompleted(difficulty) ? _completeBrush : _incompleteBrush;
        }

        private double GetIconOpacity(Difficulty difficulty)
        {
            return IsCompleted(difficulty) ? _completeOpacity : _incompleteOpacity;
        }

        private static Brush CreateFrozenBrush(string colorHex)
        {
            var color = (Color)ColorConverter.ConvertFromString(colorHex);
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

        private void RaiseDifficultyProperties()
        {
            OnPropertyChanged(nameof(CasualCompleted));
            OnPropertyChanged(nameof(StandardCompleted));
            OnPropertyChanged(nameof(IntenseCompleted));
            OnPropertyChanged(nameof(ExtremeCompleted));
            OnPropertyChanged(nameof(InsaneCompleted));

            OnPropertyChanged(nameof(CasualIconBrush));
            OnPropertyChanged(nameof(StandardIconBrush));
            OnPropertyChanged(nameof(IntenseIconBrush));
            OnPropertyChanged(nameof(ExtremeIconBrush));
            OnPropertyChanged(nameof(InsaneIconBrush));

            OnPropertyChanged(nameof(CasualIconOpacity));
            OnPropertyChanged(nameof(StandardIconOpacity));
            OnPropertyChanged(nameof(IntenseIconOpacity));
            OnPropertyChanged(nameof(ExtremeIconOpacity));
            OnPropertyChanged(nameof(InsaneIconOpacity));
        }
    }
}