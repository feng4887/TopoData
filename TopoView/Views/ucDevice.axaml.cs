// Protocol editor corresponding to TopoData.DriverPanel.ucDevice; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDevice : UserControl
{
    public ucDevice()
    {
        InitializeComponent();
        
        UiText.Apply(this);
    }

}
