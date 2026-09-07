using AFEStatViewer.Models;
using AFEStatViewer.ViewModels;

public sealed class ClassKitCompletionViewModel : ViewModelBase
{
    public ClassKitDefinition ClassKit { get; }

    public string Name => ClassKit.Name;
    public string SaveKey => ClassKit.SaveKey;

    private ModeProgress? _progress;
    public ModeProgress? Progress
    {
        get => _progress;
        set
        {
            if (!SetProperty(ref _progress, value)) return;

            OnPropertyChanged(nameof(CasualCompleted));
            OnPropertyChanged(nameof(StandardCompleted));
            OnPropertyChanged(nameof(IntenseCompleted));
            OnPropertyChanged(nameof(ExtremeCompleted));
            OnPropertyChanged(nameof(InsaneCompleted));
        }
    }

    public string MissionSaveKey { get; }

    public bool CasualCompleted => IsCompleted(Difficulty.Casual);
    public bool StandardCompleted => IsCompleted(Difficulty.Standard);
    public bool IntenseCompleted => IsCompleted(Difficulty.Intense);
    public bool ExtremeCompleted => IsCompleted(Difficulty.Extreme);
    public bool InsaneCompleted => IsCompleted(Difficulty.Insane);

    public ClassKitCompletionViewModel(
        ClassKitDefinition classKit,
        string missionSaveKey)
    {
        ClassKit = classKit;
        MissionSaveKey = missionSaveKey;
    }

    private bool IsCompleted(Difficulty difficulty)
    {
        return Progress?.GetCount(
            MissionSaveKey,
            difficulty,
            ClassKit.SaveKey
        ) > 0;
    }
}