// Interaction helpers use AXAML-backed dialogs; view layout belongs to AXAML files.
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace TopoView.Views;

public static class Ui
{
    public static int Integer(TextBox editor, string name, int minimum, int maximum) =>
        int.TryParse(editor.Text, out var value) && value >= minimum && value <= maximum ?
            value : throw new InvalidDataException(
                $"{name}必须是 {minimum} 到 {maximum} 之间的整数。");

    public static async Task MessageAsync(Control owner, string title, string message)
    {
        var dialog = new MessageDialog
        {
            Title = UiText.Translate(title), CloseButtonText = UiText.Translate("关闭")
        };
        dialog.SetMessage(message);
        await dialog.ShowAsync((Window)TopLevel.GetTopLevel(owner)!);
    }

    public static async Task<bool> ConfirmAsync(Control owner, string message)
    {
        var dialog = new MessageDialog
        {
            Title = UiText.Translate("确认操作"),
            PrimaryButtonText = UiText.Translate("确认"),
            CloseButtonText = UiText.Translate("取消")
        };
        dialog.SetMessage(message);
        return await dialog.ShowAsync((Window)TopLevel.GetTopLevel(owner)!) ==
            ContentDialogResult.Primary;
    }

    public static async Task<string?> PromptAsync(Control owner, string title,
        string initial = "")
    {
        var dialog = new MessageDialog
        {
            Title = UiText.Translate(title), PrimaryButtonText = UiText.Translate("确定"),
            CloseButtonText = UiText.Translate("取消")
        };
        dialog.SetInput(initial);
        return await dialog.ShowAsync((Window)TopLevel.GetTopLevel(owner)!) ==
            ContentDialogResult.Primary ? dialog.InputValue : null;
    }

    public static async Task<string?> OpenAsync(Control owner, string title,
        string extension)
    {
        var files = await TopLevel.GetTopLevel(owner)!.StorageProvider
            .OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title, AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType(extension)
                    { Patterns = ["*." + extension] }]
            });
        return files.FirstOrDefault()?.TryGetLocalPath();
    }

    public static async Task<string?> SaveAsync(Control owner, string title,
        string name, string extension)
    {
        var file = await TopLevel.GetTopLevel(owner)!.StorageProvider
            .SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = title, SuggestedFileName = name, DefaultExtension = extension,
                ShowOverwritePrompt = true,
                FileTypeChoices = [new FilePickerFileType(extension)
                    { Patterns = ["*." + extension] }]
            });
        return file?.TryGetLocalPath();
    }
}

