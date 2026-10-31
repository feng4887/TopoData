// Variable picker corresponding to TopoData.SubPage.diaTagSelect.
using Avalonia.Controls;
using Avalonia.Interactivity;
using auDASLib;
using TopoView.Services;

namespace TopoView.Views;

public sealed partial class diaTagSelect : Window
{
    // 原版 UpdateListView 只列出第一个通道的点位；btTnFilter_Click 则在全部点位中搜索
    private readonly List<DBTag> _allTags = [];
    private List<DBTag> _currentTagList = [];
    public string TagName { get; private set; } = "";
    public string Cannel { get; private set; } = "";
    public string UOM { get; private set; } = "";
    public string DataType { get; private set; } = "";
    public List<DBTag> tags = new();

    private readonly bool _MultiSelect;

    public diaTagSelect() { InitializeComponent(); UiText.Apply(this); }

    public diaTagSelect(WorkspaceProfile profile, bool MultiSelect = false,
        Func<DBTag, bool>? filter = null) : this()
    {
        _MultiSelect = MultiSelect;
        listView1.SelectionMode = MultiSelect ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;

        _allTags.AddRange(profile.Tags.Where(filter ?? (_ => true)).OrderBy(tag => tag.TagId));
        var cannelID = profile.Server.cDevices.Count > 0 ? profile.Server.cDevices[0].CannelID : "";
        _currentTagList = _allTags.Where(tag => tag.CannelID == cannelID).ToList();
        BindRows();
    }

    private void BindRows() =>
        listView1.ItemsSource = _currentTagList
            .Select((tag, index) => new TagRow(index, tag)).ToList();

    /// <summary>Mirrors btSelect_Click: copies the selected tags and closes.</summary>
    private void AcceptSelection()
    {
        tags.Clear();
        foreach (var selectedTag in listView1.SelectedItems.Cast<TagRow>().Select(row => row.Tag))
            tags.Add(new DBTag()
            {
                CannelID = selectedTag.CannelID,
                // 原版把 TagId 写入 TagName 交给调用方；同时保留 TagId 供移植后的页面使用
                TagId = selectedTag.TagId,
                TagName = selectedTag.TagId,
                DataType = selectedTag.DataType,
                UOM = selectedTag.UOM
            });
        Close(tags);
    }

    /// <summary>Mirrors listView1_SelectedIndexChanged: first selected row fills the public properties.</summary>
    private void listView1_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (listView1.SelectedItem is TagRow row)
        {
            Cannel = row.CannelID;
            TagName = row.TagName;
            DataType = row.DataType;
            UOM = row.UOM;
        }
    }

    /// <summary>Mirrors btTnFilter_Click: multi-keyword AND filter over all tags by TagName or TagId.</summary>
    private void btTnFilter_Click(object? sender, RoutedEventArgs e)
    {
        var strFilter = tbTnFilter.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(strFilter))
            _currentTagList = _allTags.OrderBy(t => t.TagId).ToList();
        else
        {
            var terms = strFilter.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim()).Where(t => t.Length > 0).ToArray();
            _currentTagList = _allTags
                .Where(tag => tag.TagName != null && terms.All(term =>
                    tag.TagName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    tag.TagId.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                .OrderBy(t => t.TagId).ToList();
        }
        BindRows();
    }

    private void btSelect_Click(object? sender, RoutedEventArgs e) => AcceptSelection();

    private void listView1_MouseDoubleClick(object? sender, RoutedEventArgs e) => AcceptSelection();

    /// <summary>Row model mirrors listView1_RetrieveVirtualItem: zero-based index plus the tag text.</summary>
    public sealed record TagRow(int Index, DBTag Tag)
    {
        public string CannelID => Tag.CannelID ?? "";
        public string TagName => Tag.TagId ?? "";
        public string DataType => DBTag.DataTypeString(Tag.DataType);
        public string UOM => Tag.UOM ?? "";
    }
}
