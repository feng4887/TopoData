// Single-tag write editor corresponding to TopoData.SubPage.diaWrite.
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TopoView.Views;

public sealed partial class diaWrite : Window
{
    public string strValue { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string UOM { set => tbAddress.Text = value; }
    public string TagName { get => tbName.Text ?? ""; set => tbName.Text = value; }

    public diaWrite() { InitializeComponent(); UiText.Apply(this); }

    private void btWrite_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(tbValue.Text))
        {
            _ = Ui.MessageAsync(this, "Information", "Value cannot be empty.");
            return;
        }

        strValue = tbValue.Text;
        Close(true);
    }

    private void btCancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
