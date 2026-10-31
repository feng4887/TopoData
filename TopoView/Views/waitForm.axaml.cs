// Progress window used by the original history-query workflow.
using Avalonia.Controls;

namespace TopoView.Views;

public sealed partial class waitForm : Window
{
    public waitForm() => InitializeComponent();
    public void SetCaption(string caption) => progressPanel1.Header = caption;
    public void SetDescription(string description) => progressPanel1.SetValue(ToolTip.TipProperty, description);
}
