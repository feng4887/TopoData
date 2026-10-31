// Compares the Avalonia business controls against the original TopoData Designer files.
using System.Text.RegularExpressions;
using System.Xml.Linq;

internal static class UiParityChecks
{
    private static readonly HashSet<string> Components =
    [
        "GridView", "ImageList", "ImageCollection", "Timer", "FluentFormDefaultManager",
        "RepositoryItemComboBox", "RepositoryItemImageComboBox", "BarManager", "BarDockControl"
    ];

    public static void Run(Action<bool, string> check)
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root != null && !Directory.Exists(Path.Combine(root.FullName, "TopoData", "SubPage")))
            root = root.Parent;
        if (root == null) throw new InvalidOperationException("TopoData source is required for UI parity checks.");
        string[] pages = ["frmMain", "ucCannelCfg", "ucDataTableCfg", "ucRecipe", "ucScripts",
            "ucDiagnose", "ucHisQuery", "ucSystemCfg", "ucAbout", "ucDriverOPCUA",
            "ucDriverProfinet", "ucDriverModbusTCP", "ucDriverModbusRTU", "ucDriverEmpty", "ucDevice",
            "diaOPCUATagSelect", "diaTagSelect", "diaExpress", "diaExpressAbout", "diaExpressCheck",
            "diaColumnSelect", "diaRecipeTagSelect", "diaWrite", "waitForm"];
        foreach (var page in pages)
        {
            var source = Directory.GetFiles(Path.Combine(root.FullName, "TopoData"),
                page + ".Designer.cs", SearchOption.AllDirectories).Single();
            var code = RemoveComments(File.ReadAllText(source));
            var declared = Regex.Matches(code,
                @"\b(?:private|public|internal|protected)\s+[\w.<>]+\s+(\w+)\s*(?:;|=\s*null\s*;)")
                .Select(match => match.Groups[1].Value).ToHashSet();
            var expected = Regex.Matches(code,
                @"^\s*(?:this\.)?(\w+)\s*=\s*new\s+([\w.<>]+)\s*\(", RegexOptions.Multiline)
                .Where(match => declared.Contains(match.Groups[1].Value) && match.Groups[1].Value != "components")
                .ToDictionary(match => match.Groups[1].Value, match => match.Groups[2].Value.Split('.').Last());
            foreach (var entry in expected.Where(entry => Components.Contains(entry.Value) ||
                entry.Value == "Bar" && entry.Key != "bar2").ToList()) expected.Remove(entry.Key);
            if (page == "ucCannelCfg")
            {
                expected.Add("ucOPCUA", "ucDriverOPCUA");
                expected.Add("ucProfinet", "ucDriverProfinet");
                expected.Add("ucModbusTCP", "ucDriverModbusTCP");
                expected.Add("ucModbusRTU", "ucDriverModbusRTU");
                expected.Add("ucDriverEmpty", "ucDriverEmpty");
            }
            var target = Path.Combine(root.FullName, "TopoView",
                page == "frmMain" ? "frmMain.axaml" : Path.Combine("Views", page + ".axaml"));
            var document = XDocument.Load(target);
            XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
            var controls = document.Root!.DescendantsAndSelf().Select(element =>
                (Element: element, Name: (string?)element.Attribute(x + "Name") ??
                    (element.Name.LocalName is "LegacyTextColumn" or "LegacyTemplateColumn"
                        ? (string?)element.Attribute("Name") : null)))
                .Where(entry => entry.Name != null).ToDictionary(entry => entry.Name!, entry => entry.Element);
            var missing = expected.Keys.Except(controls.Keys).ToArray();
            var extra = controls.Keys.Except(expected.Keys).ToArray();
            check(missing.Length == 0 && extra.Length == 0,
                $"TopoData UI names/counts match {page}: {expected.Count}; missing={string.Join(',', missing)}; extra={string.Join(',', extra)}");
            var mismatch = expected.Where(entry => !Compatible(entry.Value, controls[entry.Key].Name.LocalName)).ToArray();
            check(mismatch.Length == 0, "TopoData UI control roles match " + page + ": " +
                string.Join(',', mismatch.Select(entry => entry.Key + "=" + entry.Value + "/" + controls[entry.Key].Name.LocalName)));
            var targetCode = File.ReadAllText(target + ".cs");
            var actions = controls.Values.Where(element => element.Name.LocalName is "Button" or "MenuItem").ToList();
            check(actions.All(element => element.Attribute("Click") is { } handler &&
                Regex.IsMatch(targetCode, @"\b" + Regex.Escape(handler.Value) + @"\s*\(")),
                "TopoData UI action controls have named event handlers: " + page);
        }
    }

    private static string RemoveComments(string text) => Regex.Replace(text,
        "\"(?:\\\\.|[^\"\\\\])*\"|//[^\r\n]*|/\\*[\\s\\S]*?\\*/",
        match => match.Value.StartsWith('"') ? match.Value : "");

    private static bool Compatible(string source, string target) => source switch
    {
        "SimpleButton" or "BarButtonItem" or "Button" => target == "Button",
        "Label" or "LabelControl" or "BarStaticItem" => target == "TextBlock",
        "TextEdit" or "MemoEdit" or "TextBox" or "RichTextBox" => target == "TextBox",
        // The Scintilla Python editor is ported to AvaloniaEdit's TextEditor for syntax highlighting.
        "Scintilla" => target is "TextBox" or "TextEditor",
        // The history query keeps its timestamp editable through a calendar and clock pair.
        "DateTimeOffsetEdit" => target is "TextBox" or "LegacyDateTimeEdit",
        "ComboBoxEdit" or "ComboBox" or "RadioGroup" or "BarEditItem" => target == "ComboBox",
        "ToggleSwitch" or "CheckEdit" or "RadioButton" => target is "CheckBox" or "RadioButton",
        "GridControl" or "NoFlashListView" or "ListView" or "DataGridView" => target == "DataGrid",
        "GridColumn" or "ColumnHeader" or "DataGridViewTextBoxColumn" => target is "LegacyTextColumn" or "LegacyTemplateColumn",
        "GroupControl" or "GroupBox" or "NavBarGroup" or "ProgressPanel" => target == "HeaderedContentControl",
        "PictureEdit" or "PictureBox" => target == "Image",
        "NavBarControl" => target == "ListBox",
        "NavBarItem" => target == "ListBoxItem",
        "AccordionControl" => target == "NavigationView",
        "AccordionControlElement" => target == "NavigationViewItem",
        "WITPLMMultiSelectTreeView" => target == "TreeView",
        "ContextMenuStrip" => target == "ContextMenu",
        "ToolStripMenuItem" => target == "MenuItem",
        "ToolStripSeparator" => target == "Separator",
        _ => source.StartsWith("uc", StringComparison.Ordinal) ? source == target :
            target is "Grid" or "Border" or "StackPanel" or "WrapPanel" or "Panel"
    };
}
