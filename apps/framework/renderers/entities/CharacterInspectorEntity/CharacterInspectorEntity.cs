using Godot;
using DiceRolling.Events;
using DiceRolling.Characters;
using DiceRolling.Components;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
public partial class CharacterInspectorEntity : EntityControl {
    public CharacterType? InspectedCharacterData => GetData<CharacterType>();

    [ExportGroup("Preview")]
    [Export] public CharacterType? PreviewCharacterData { get; set; }

    [ExportToolButton("Update Preview")]
    public Callable UpdatePreviewData => Callable.From(() => {
        if (Engine.IsEditorHint()) {
            Data = PreviewCharacterData;
            NotifyUpdate();
            Visible = Data != null;
        }

    });

    public override void _Ready() {
        if (Engine.IsEditorHint()) {
            if (PreviewCharacterData != null && Data != PreviewCharacterData) {
                Data = PreviewCharacterData;
                Visible = true;
            }
            else if (PreviewCharacterData == null) {
                Data = null;
                Visible = false;
            }
        }
        else {
            Visible = false;
            if (EventBus.Instance != null) {
                EventBus.Instance.ComponentSelected += OnComponentSelected;
                EventBus.Instance.ComponentUnselected += OnComponentUnselected;
            }
            else {
                GD.PrintErr("EventBus instance not found at runtime.");
            }
        }
    }

    public override void _ExitTree() {
        if (!Engine.IsEditorHint() && EventBus.Instance != null) {
            EventBus.Instance.ComponentSelected -= OnComponentSelected;
            EventBus.Instance.ComponentUnselected -= OnComponentUnselected;
        }
    }

    private void OnComponentSelected(Node component) {
        CharacterType? newCharacterData = null;

        if (component is SelectableComponent selectableComponent) {
            var parentEntity = selectableComponent.GetParentOrNull<Entity3D>();
            if (parentEntity is CharacterEntity characterEntity && characterEntity.CharacterData is CharacterType characterData) {
                newCharacterData = characterData;
            }
        }

        if (Data != newCharacterData) {
            Data = newCharacterData;
            Visible = Data != null;

            if (Data != null) {
                GD.Print($"CharacterInspectorEntity: Selected {((CharacterType)Data).Name}");
            }
            else {
                GD.Print($"CharacterInspectorEntity: Selection cleared.");
            }
        }
    }

    private void OnComponentUnselected(Node component) {
        if (component is SelectableComponent selectableComponent) {
            var parentEntity = selectableComponent.GetParentOrNull<Entity3D>();
            if (parentEntity is CharacterEntity characterEntity && characterEntity.CharacterData == Data) {
                if (Data != null) {
                    GD.Print($"CharacterInspectorEntity: Unselected {((CharacterType)Data).Name}");
                    Data = null;
                    Visible = false;
                }
            }
        }
    }
}