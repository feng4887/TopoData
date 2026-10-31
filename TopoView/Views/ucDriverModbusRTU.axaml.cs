// Protocol editor corresponding to TopoData.DriverPanel.ucDriverModbusRTU; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDriverModbusRTU : UserControl
{
    public ucDriverModbusRTU()
    {
        InitializeComponent();
        cbComPort.ItemsSource = new[] { OperatingSystem.IsWindows() ? "COM1" : "/dev/ttyUSB0" };
        cbComPort.SelectedIndex = 0;
        cbBaudRate.ItemsSource = new[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
        cbBaudRate.SelectedItem = 9600;
        cbParity.ItemsSource = Enum.GetValues<Parity>(); cbParity.SelectedItem = Parity.None;
        cbDataBits.ItemsSource = new[] { 5, 6, 7, 8 }; cbDataBits.SelectedItem = 8;
        cbStopBits.ItemsSource = Enum.GetValues<StopBits>().Where(x => x != StopBits.None).ToArray();
        cbStopBits.SelectedItem = StopBits.One;
        cbParseData.ItemsSource = ucDriverModbusTCP.ByteOrders; cbParseData.SelectedIndex = 0;
        UiText.Apply(this);
    }

    public void LoadConfig(ComCfg serial, IpCfg ip)
    {
        cbComPort.ItemsSource = SerialPort.GetPortNames().Append(serial.PortName)
            .Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();
        cbComPort.SelectedItem = serial.PortName;
        cbBaudRate.ItemsSource = new[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 }
            .Append(serial.BaudRate).Distinct().Order().ToArray();
        cbBaudRate.SelectedItem = serial.BaudRate;
        cbParity.SelectedItem = serial.Parity;
        cbDataBits.SelectedItem = serial.DataBits;
        cbStopBits.SelectedItem = serial.StopBits;
        cbParseData.SelectedIndex = Math.Clamp(ip.Rack, 0, 3);
        tbDeviceAddress.Text = ip.Slot.ToString();
    }

    public ComCfg ReadSerialConfig()
    {
        var cfg = new ComCfg
        {
            PortName = cbComPort.SelectedItem as string ?? "",
            BaudRate = (int)(cbBaudRate.SelectedItem ?? 9600),
            Parity = (Parity)(cbParity.SelectedItem ?? Parity.None),
            DataBits = (int)(cbDataBits.SelectedItem ?? 8),
            StopBits = (StopBits)(cbStopBits.SelectedItem ?? StopBits.One)
        };
        if (!cfg.Validate(out var error)) throw new InvalidDataException(error);
        return cfg;
    }

    public IpCfg ReadAddressConfig(IpCfg original) => new()
    {
        Host = original.Host, Port = original.Port,
        Rack = Math.Max(0, cbParseData.SelectedIndex),
        Slot = Ui.Integer(tbDeviceAddress, "Device Addr", 0, 255)
    };
}
