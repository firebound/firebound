using Godot;
using DiceRolling.Characters;
using DiceRolling.Events;

namespace DiceRolling.Components.CharacterInspector;

/// <summary>
/// A button component representing a single character action in the inspector.
/// Displays the action name and emits an ActionSelected signal when pressed.
/// </summary>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterActionButtonComponent : Button {
    private CharacterAction? _actionData;

    public CharacterAction? ActionData {
        get => _actionData;
        set {
            _actionData = value;
            UpdateDisplay();
        }
    }

    public override void _Ready() {
        Pressed += OnButtonPressed;
        UpdateDisplay();
    }

    private void UpdateDisplay() {
        Text = _actionData?.Type?.Name ?? "No Action";
        // Disable the button if there's no valid action or target board
        Disabled = _actionData?.Type?.TargetBoard == null;
    }

    private void OnButtonPressed() {
        if (_actionData?.Type?.TargetBoard != null) {
            GD.Print($"[CharacterActionButtonComponent] Action '{_actionData.Type.Name}' selected.");
            EventBus.Instance.EmitActionSelected(_actionData.Type.TargetBoard);
        }
        else {
            GD.PrintErr($"[CharacterActionButtonComponent] Action '{_actionData?.Type?.Name ?? "Unknown"}' has no TargetBoard configured.");
        }
    }
}