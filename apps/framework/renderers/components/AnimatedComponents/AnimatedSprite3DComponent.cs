using Godot;
using DiceRolling.Entities;
using DiceRolling.Characters;

namespace DiceRolling.Components;

/// <summary>
/// Component that displays an animated sprite for a character entity.
/// </summary>
/// <remarks>
/// This component listens for updates to the parent Entity3D's CharacterType and updates the sprite accordingly.
/// </remarks>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-3d.svg")]
public partial class AnimatedSprite3DComponent : AnimatedSprite3D {
    private Entity3D? _parent;

    public override void _Ready() {
        _parent = GetParent<Entity3D>();

        if (_parent != null) {
            _parent.EntityUpdated += OnEntityUpdated;
            UpdateSprite();
        }
    }

    public override void _ExitTree() {
        if (_parent != null) {
            _parent.EntityUpdated -= OnEntityUpdated;
        }
    }

    private void OnEntityUpdated() {
        GD.Print($"[AnimatedSprite3DComponent] Entity Updated: {_parent?.Data?.Id}");
        UpdateSprite();
    }

    private void UpdateSprite() {
        if (_parent == null) return;

        var characterData = _parent.GetData<CharacterType>();
        if (characterData == null) return;

        if (characterData.CharacterSprite != null) {
            SpriteFrames = characterData.CharacterSprite;
        }

        PixelSize = characterData.PixelSize;

        Offset = new Vector2(
            characterData.SpritePositionX,
            characterData.SpritePositionY
        );

        Play("idle");

    }
}