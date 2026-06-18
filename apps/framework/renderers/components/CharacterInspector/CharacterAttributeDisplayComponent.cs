using Godot;
using DiceRolling.Characters; // Required for CharacterAttribute

namespace DiceRolling.Components.CharacterInspector;

/// <summary>
/// Displays the details of a single character attribute (Name, Current Value, Max Value).
/// </summary>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterAttributeDisplayComponent : VBoxContainer {
    private CharacterAttribute? _attributeData;

    [ExportGroup("Nodes")]
    [Export] public Label? NameLabel { get; set; }
    [Export] public Label? CurrentValueLabel { get; set; }
    [Export] public Label? MaxValueLabel { get; set; }
    public CharacterAttribute? AttributeData {
        get => _attributeData;
        set {
            _attributeData = value;
            UpdateDisplay();
        }
    }

    public override void _Ready() {
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        if (_attributeData?.Type != null) {
            if (NameLabel != null) {
                NameLabel.Text = _attributeData.Type.Name ?? "Unknown Attribute";
            }
            if (CurrentValueLabel != null) {
                CurrentValueLabel.Text = _attributeData.CurrentValue.ToString();
            }
            if (MaxValueLabel != null) {
                MaxValueLabel.Text = _attributeData.MaxValue.ToString();
            }
            Visible = true;
        }
        else {
            if (NameLabel != null) NameLabel.Text = "";
            if (CurrentValueLabel != null) CurrentValueLabel.Text = "";
            if (MaxValueLabel != null) MaxValueLabel.Text = "";
            Visible = false;
        }
    }
}