using Godot;
using DiceRolling.Characters;
using DiceRolling.Entities;
using DiceRolling.Helpers;
using System.Collections.Generic; // Required for List

namespace DiceRolling.Components.CharacterInspector;

/// <summary>
/// Displays a list of actions for the currently inspected character using buttons.
/// It listens to the parent CharacterInspectorEntity's EntityUpdated signal
/// and manages instances of CharacterActionButtonComponent.
/// </summary>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterActionListComponent : GridContainer {
    private CharacterInspectorEntity? _inspectorEntity;
    private List<Node> _actionButtonInstances = new();

    [ExportGroup("Configuration")]
    [Export] public PackedScene? ActionButtonScene { get; set; }

    public override void _Ready() {
        _inspectorEntity = this.FindAncestorOfType<CharacterInspectorEntity>();

        if (_inspectorEntity == null) {
            GD.PrintErr($"{nameof(CharacterActionListComponent)} could not find an ancestor of type {nameof(CharacterInspectorEntity)}.");
            return;
        }

        _inspectorEntity.EntityUpdated += OnEntityUpdated;

        OnEntityUpdated();
    }

    public override void _ExitTree() {
        if (_inspectorEntity != null) {
            _inspectorEntity.EntityUpdated -= OnEntityUpdated;
        }
        ClearActionButtons();
    }

    private void OnEntityUpdated() {
        var characterData = _inspectorEntity?.GetData<CharacterType>();
        UpdateActionList(characterData);
    }

    private void UpdateActionList(CharacterType? characterData) {
        ClearActionButtons();

        if (ActionButtonScene == null) {
            GD.PrintErr($"{nameof(ActionButtonScene)} is not set in {nameof(CharacterActionListComponent)}.");
            Visible = false;
            return;
        }

        if (characterData?.Actions != null && characterData.Actions.Count > 0) {
            foreach (var action in characterData.Actions) {
                if (action?.Type == null) continue;

                var actionButtonInstance = ActionButtonScene.Instantiate();
                if (actionButtonInstance is CharacterActionButtonComponent buttonComponent) {
                    buttonComponent.ActionData = action;
                    AddChild(actionButtonInstance);
                    _actionButtonInstances.Add(actionButtonInstance);
                }
                else {
                    GD.PrintErr($"Instantiated scene from {nameof(ActionButtonScene)} does not have a {nameof(CharacterActionButtonComponent)} script attached.");
                    actionButtonInstance.QueueFree();
                }
            }
            Visible = true;
        }
        else {
            Visible = false;
        }
    }

    private void ClearActionButtons() {
        foreach (var instance in _actionButtonInstances) {
            if (IsInstanceValid(instance)) {
                instance.QueueFree();
            }
        }
        _actionButtonInstances.Clear();
    }
}