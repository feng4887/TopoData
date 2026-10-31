// Protocol editor corresponding to TopoData.DriverPanel.ucDriverModbusTCP; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDriverModbusTCP : UserControl
{
    public ucDriverModbusTCP()
    {
        InitializeComponent();
        cbParseData.ItemsSource = ByteOrders; cbParseData.SelectedIndex = 0;
        UiText.Apply(this);
    }

    internal static readonly string[] ByteOrders = ["CDAB", "ABCD", "BADC", "DCBA"];

    /// <summary>Mirrors the original ucDriverModbusTCP.Host used by the save validation.</summary>
    public string Host => tbIP.Text ?? "";

    public void LoadConfig(IpCfg cfg)
    {
        tbIP.Text = cfg.Host;
        tbPort.Text = cfg.Port.ToString();
        cbParseData.SelectedIndex = Math.Clamp(cfg.Rack, 0, 3);
        tbDeviceAddress.Text = cfg.Slot.ToString();
    }

    public IpCfg ReadConfig() => new()
    {
        Host = tbIP.Text ?? "",
        Port = Ui.Integer(tbPort, "Port", 1, 65535),
        Rack = Math.Max(0, cbParseData.SelectedIndex),
        Slot = Ui.Integer(tbDeviceAddress, "Device ID", 0, 255)
    };
}
