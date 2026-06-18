using Godot;
using DiceRolling.Characters;
using DiceRolling.Entities;
using DiceRolling.Helpers;

namespace DiceRolling.Components.CharacterInspector;

/// <summary>
/// Displays the name of the currently inspected character.
/// It listens to the parent CharacterInspectorEntity's EntityUpdated signal.
/// </summary>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterNameComponent : Label {
    private CharacterInspectorEntity? _inspectorEntity;

    public override void _Ready() {
        _inspectorEntity = this.FindAncestorOfType<CharacterInspectorEntity>();

        if (_inspectorEntity == null) {
            GD.PrintErr($"{nameof(CharacterNameComponent)} could not find an ancestor of type {nameof(CharacterInspectorEntity)}.");
            return;
        }

        _inspectorEntity.EntityUpdated += OnEntityUpdated;

        OnEntityUpdated();
    }

    public override void _ExitTree() {
        if (_inspectorEntity != null) {
            _inspectorEntity.EntityUpdated -= OnEntityUpdated;
        }
    }

    private void OnEntityUpdated() {
        var characterData = _inspectorEntity?.GetData<CharacterType>();

        if (characterData != null) {
            Text = characterData.Name ?? "Unknown Name";
            Visible = true;
        }
        else {
            Text = "";
            Visible = false;
        }
    }
}