// Protocol editor corresponding to TopoData.DriverPanel.ucDriverEmpty; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDriverEmpty : UserControl
{
    public ucDriverEmpty()
    {
        InitializeComponent();
        
        UiText.Apply(this);
    }

}
