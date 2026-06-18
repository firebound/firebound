using Godot;

namespace DiceRolling.Characters;

/// <summary>
/// Define os recursos visuais de um personagem.
/// </summary>
public interface ICharacterAssetSheet {
    Texture2D? Portrait { get; set; }
    SpriteFrames? CharacterSprite { get; set; }
    float PixelSize { get; set; }
    SpriteFrames? ShadowSprite { get; set; }
    bool ShowShadow { get; set; }
    float SpritePositionX { get; set; }
    float SpritePositionY { get; set; }
}