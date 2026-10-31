// Keep FluentAvalonia control names explicit when importing Avalonia controls.
global using NavigationViewItem = FluentAvalonia.UI.Controls.NavigationViewItem;
global using Symbol = FluentAvalonia.UI.Controls.Symbol;
global using SymbolIconSource = FluentAvalonia.UI.Controls.SymbolIconSource;
global using ContentDialog = FluentAvalonia.UI.Controls.ContentDialog;
global using ContentDialogResult = FluentAvalonia.UI.Controls.ContentDialogResult;

// Bind the TopoView/Public helper classes to the local copies instead of the
// same-named types that hiLib (auDASLib) and hiServer (auDAServer) provide.
// Note: DataImport must stay on hiLib's auDASLib.DataImport — hiServer's
// ItemPool.Start builds its TagPool from that class's static dictionaries.
global using PubFunction = TopoView.Public.PubFunction;
global using UAClientHelperAPI = TopoView.Public.UAClientHelperAPI;
global using PythonScriptReadResolver = TopoView.Public.PythonScriptReadResolver;
