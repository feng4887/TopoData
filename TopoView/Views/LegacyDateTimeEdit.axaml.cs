// Editable timestamp with a calendar/clock dropdown; text is normalised when the field is left.
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;

namespace TopoView.Views;

public sealed partial class LegacyDateTimeEdit : UserControl
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public static readonly StyledProperty<DateTime?> ValueProperty =
        AvaloniaProperty.Register<LegacyDateTimeEdit, DateTime?>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public LegacyDateTimeEdit()
    {
        InitializeComponent();
        // The field is committed on leave, so typing is never reformatted mid-edit.
        editor.LostFocus += (_, _) => CommitEditor();
        editor.KeyDown += (_, e) => { if (e.Key == Key.Enter) CommitEditor(); };
        calendar.SelectedDatesChanged += (_, _) => ReadPicker();
        timePicker.SelectedTimeChanged += (_, _) => ReadPicker();
        Value = DateTime.Now;
    }

    /// <summary>Timestamp held by the field; the dropdown parts follow it.</summary>
    public DateTime? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property != ValueProperty) return;
        var value = Value ?? DateTime.Now;
        editor.Text = value.ToString(Format);
        calendar.SelectedDate = value.Date;
        timePicker.SelectedTime = value.TimeOfDay;
    }

    private void CommitEditor()
    {
        if (DateTime.TryParse(editor.Text, out var value)) SetCurrentValue(ValueProperty, value);
        editor.Text = (Value ?? DateTime.Now).ToString(Format);
    }

    private void ReadPicker() => SetCurrentValue(ValueProperty,
        (calendar.SelectedDate ?? DateTime.Today).Add(timePicker.SelectedTime ?? TimeSpan.Zero));
}
