// Protocol editor corresponding to TopoData.DriverPanel.ucDriverProfinet; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDriverProfinet : UserControl
{
    public ucDriverProfinet()
    {
        InitializeComponent();
        
        UiText.Apply(this);
    }

    /// <summary>Mirrors the original ucDriverProfinet.Host used by the save validation.</summary>
    public string Host => tbStationIPMain.Text ?? "";

    public void LoadConfig(IpCfg cfg)
    {
        tbStationIPMain.Text = cfg.Host;
        tbStationRackMain.Text = cfg.Rack.ToString();
        tbStationSlotMain.Text = cfg.Slot.ToString();
    }

    public IpCfg ReadConfig(IpCfg original) => new()
    {
        Host = tbStationIPMain.Text ?? "",
        Port = original.Port,
        Rack = Ui.Integer(tbStationRackMain, "Rack", 0, 255),
        Slot = Ui.Integer(tbStationSlotMain, "Slot", 0, 255)
    };
}
