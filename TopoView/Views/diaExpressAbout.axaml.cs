// Expression help corresponding to TopoData.SubPage.diaExpressAbout.
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TopoView.Views;

public sealed partial class diaExpressAbout : Window
{
    public diaExpressAbout() { InitializeComponent(); UiText.Apply(this); }
    private void btYes_Click(object? sender, RoutedEventArgs e) => Close();
}
