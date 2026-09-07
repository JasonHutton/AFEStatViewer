namespace AFEStatViewer.Models
{
    public sealed record ClassKitVisualStyle(
        string CompleteColorHex,
        string IncompleteColorHex,
        string IconBackgroundColorHex,
        string CompletionSectionBackgroundColorHex,
        string CompletionTextColorHex,
        double CompleteOpacity = 1.0,
        double IncompleteOpacity = 0.35,
        double IconBackgroundOpacity = 1.0,
        double IconSize = 18.0
    );
}