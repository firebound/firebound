using Godot;
using DiceRolling.Characters;
using DiceRolling.Entities;
using DiceRolling.Helpers;
using System.Collections.Generic; // Required for List

namespace DiceRolling.Components.CharacterInspector;

/// <summary>
/// Displays a list of attributes for the currently inspected character.
/// It listens to the parent CharacterInspectorEntity's EntityUpdated signal
/// and manages instances of CharacterAttributeDisplayComponent.
/// </summary>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-ui.svg")]
public partial class CharacterAttributeListComponent : VBoxContainer {
    private CharacterInspectorEntity? _inspectorEntity;
    private List<Node> _attributeDisplayInstances = new();

    [ExportGroup("Configuration")]
    [Export] public PackedScene? AttributeDisplayScene { get; set; }

    public override void _Ready() {
        _inspectorEntity = this.FindAncestorOfType<CharacterInspectorEntity>();

        if (_inspectorEntity == null) {
            GD.PrintErr($"{nameof(CharacterAttributeListComponent)} could not find an ancestor of type {nameof(CharacterInspectorEntity)}.");
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
        ClearAttributeDisplays();
    }

    private void OnEntityUpdated() {
        var characterData = _inspectorEntity?.GetData<CharacterType>();
        UpdateAttributeList(characterData);
    }

    private void UpdateAttributeList(CharacterType? characterData) {
        ClearAttributeDisplays();

        if (AttributeDisplayScene == null) {
            GD.PrintErr($"{nameof(AttributeDisplayScene)} is not set in {nameof(CharacterAttributeListComponent)}.");
            Visible = false;
            return;
        }

        if (characterData?.Attributes != null && characterData.Attributes.Count > 0) {
            foreach (var attribute in characterData.Attributes) {
                if (attribute == null) continue;

                var attributeInstance = AttributeDisplayScene.Instantiate();
                if (attributeInstance is CharacterAttributeDisplayComponent displayComponent) {
                    displayComponent.AttributeData = attribute;
                    AddChild(attributeInstance);
                    _attributeDisplayInstances.Add(attributeInstance);
                }
                else {
                    GD.PrintErr($"Instantiated scene from {nameof(AttributeDisplayScene)} does not have a {nameof(CharacterAttributeDisplayComponent)} script attached.");
                    attributeInstance.QueueFree();
                }
            }
            Visible = true;
        }
        else {
            Visible = false;
        }
    }

    private void ClearAttributeDisplays() {
        foreach (var instance in _attributeDisplayInstances) {
            if (IsInstanceValid(instance)) {
                instance.QueueFree();
            }
        }
        _attributeDisplayInstances.Clear();
    }
}