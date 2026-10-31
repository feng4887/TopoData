// Protocol editor corresponding to TopoData.DriverPanel.ucDriverOPCUA; AXAML owns its controls.
using Avalonia.Controls;
using System.IO.Ports;
using Avalonia.Interactivity;
using auDASLib;
using Opc.Ua;

namespace TopoView.Views;

public sealed partial class ucDriverOPCUA : UserControl
{
    public ucDriverOPCUA()
    {
        InitializeComponent();
        
        UiText.Apply(this);
    }

    private UACfg _original = new();
    public event EventHandler? DiscoverRequested;
    public event EventHandler? BrowseRequested;
    public string EndpointUrl => tbEndPoint.Text ?? "";

    public void LoadConfig(UACfg cfg)
    {
        _original = cfg;
        tbEndPoint.Text = cfg.EndpointUrl;
        tbUserName.Text = cfg.User;
        tbPsw.Text = cfg.Psw;
        cbUseUserLogIn.IsChecked = cfg.UseUserLogIn;
        cbEndpointItems.ItemsSource = new[] { new EndpointChoice(null, cfg.SecurityMode, cfg.securityPolicy) };
        cbEndpointItems.SelectedIndex = 0;
        UpdateLogin();
    }

    public void SetEndpoints(IEnumerable<EndpointDescription> endpoints)
    {
        cbEndpointItems.ItemsSource = endpoints.Select(endpoint =>
            new EndpointChoice(endpoint, endpoint.SecurityMode.ToString(),
                endpoint.SecurityPolicyUri.Split('#').Last())).ToList();
        cbEndpointItems.SelectedIndex = 0;
    }

    public UACfg ReadConfig()
    {
        var selected = cbEndpointItems.SelectedItem as EndpointChoice;
        return new UACfg
        {
            EndpointUrl = EndpointUrl,
            SecurityMode = selected?.SecurityMode ?? _original.SecurityMode,
            securityPolicy = selected?.SecurityPolicy ?? _original.securityPolicy,
            User = tbUserName.Text ?? "", Psw = tbPsw.Text ?? "",
            UseUserLogIn = cbUseUserLogIn.IsChecked == true,
            Subscribe = _original.Subscribe, ApplicationName = _original.ApplicationName
        };
    }

    private void btGetEndpoint_Click(object? sender, RoutedEventArgs e) => DiscoverRequested?.Invoke(sender, e);
    private void btGetTag_Click(object? sender, RoutedEventArgs e) => BrowseRequested?.Invoke(sender, e);
    private void cbEndpointItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (cbEndpointItems.SelectedItem is EndpointChoice { Endpoint: { } endpoint })
            tbEndPoint.Text = endpoint.EndpointUrl;
    }
    private void cbUseUserLogIn_IsCheckedChanged(object? sender, RoutedEventArgs e) => UpdateLogin();
    private void UpdateLogin()
    {
        if (tbUserName == null || tbPsw == null) return;
        tbUserName.IsEnabled = tbPsw.IsEnabled = cbUseUserLogIn.IsChecked == true;
    }

    private sealed record EndpointChoice(EndpointDescription? Endpoint, string SecurityMode, string SecurityPolicy)
    {
        public override string ToString() => $"[{SecurityMode}] [{SecurityPolicy}] {Endpoint?.EndpointUrl}";
    }
}
