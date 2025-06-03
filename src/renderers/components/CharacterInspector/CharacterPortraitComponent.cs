using Godot;
using DiceRolling.Characters;
using DiceRolling.Entities;
using DiceRolling.Helpers;

namespace DiceRolling.Components.CharacterInspector;

[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterPortraitComponent : TextureRect {
    private CharacterInspectorEntity? _inspectorEntity;

    public override void _Ready() {
        _inspectorEntity = this.FindAncestorOfType<CharacterInspectorEntity>();

        if (_inspectorEntity == null) {
            GD.PrintErr($"{nameof(CharacterPortraitComponent)} could not find an ancestor of type {nameof(CharacterInspectorEntity)}.");
            return;
        }

        _inspectorEntity.EntityUpdated += OnEntityUpdated;

        // Initial update
        OnEntityUpdated();
    }

    public override void _ExitTree() {
        if (_inspectorEntity != null) {
            _inspectorEntity.EntityUpdated -= OnEntityUpdated;
        }
    }

    private void OnEntityUpdated() {
        var characterData = _inspectorEntity?.GetData<CharacterType>();

        if (characterData?.Portrait != null) {
            Texture = characterData.Portrait;
            Visible = true;
        }
        else {
            Texture = null;
            Visible = false;
        }
    }
}