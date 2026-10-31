using System.Xml;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;

namespace TopoView.Views;

/// <summary>
/// Python syntax definition for the script editor, loaded from Assets/Python-Mode.xshd.
/// Colors mirror the Scintilla lexer configuration of TopoData.SubPage.ucScripts.
/// </summary>
internal static class PythonHighlighting
{
    public static IHighlightingDefinition Definition { get; } = Load();

    private static IHighlightingDefinition Load()
    {
        var assembly = typeof(PythonHighlighting).Assembly;
        using var stream = assembly.GetManifestResourceStream("TopoView.Assets.Python-Mode.xshd");
        using var reader = XmlReader.Create(stream!);
        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }
}
