// Modal message and input dialog shared by explicit application actions. A real
// borderless window (no dim overlay) that moves anywhere on screen by its title bar.
using Avalonia.Controls;
using Avalonia.Input;
using FluentAvalonia.UI.Controls;

namespace TopoView.Views;

public sealed partial class MessageDialog : Window
{
    public MessageDialog()
    {
        InitializeComponent();
        UiText.Apply(this);
        btPrimary.Click += (_, _) => Close(ContentDialogResult.Primary);
        btSecondary.Click += (_, _) => Close(ContentDialogResult.Secondary);
        btClose.Click += (_, _) => Close(ContentDialogResult.None);
        TitleBar.PointerPressed += (_, e) =>
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) BeginMoveDrag(e);
        };
    }

    public string InputValue => txtValue.Text ?? "";

    /// <summary>Shows the dialog modally over the owner and returns the pressed button.
    /// Closing without a button (Esc/Alt+F4) yields ContentDialogResult.None.</summary>
    public Task<ContentDialogResult> ShowAsync(Window owner) =>
        ShowDialog<ContentDialogResult>(owner);

    public string PrimaryButtonText
    {
        get => btPrimary.Content as string ?? "";
        set { btPrimary.Content = value; btPrimary.IsVisible = !string.IsNullOrEmpty(value); }
    }

    public string SecondaryButtonText
    {
        get => btSecondary.Content as string ?? "";
        set { btSecondary.Content = value; btSecondary.IsVisible = !string.IsNullOrEmpty(value); }
    }

    public string CloseButtonText
    {
        get => btClose.Content as string ?? "";
        set { btClose.Content = value; btClose.IsVisible = !string.IsNullOrEmpty(value); }
    }

    public void SetMessage(string message) => lblMessage.Text = message;

    public void SetInput(string initialValue)
    {
        lblMessage.IsVisible = false;
        txtValue.IsVisible   = true;
        txtValue.Text        = initialValue;
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        (btPrimary.IsVisible ? btPrimary : btClose).Focus();
    }
}
