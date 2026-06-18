using DiceRolling.Attributes;

namespace DiceRolling.Characters;
/// <summary>
/// Define os atributos de um personagem.
/// </summary>
public interface ICharacterAttributeSheet {
    Godot.Collections.Array<CharacterAttribute> Attributes { get; }
    void InitializeAttributes();
    int GetAttributeCurrentValue(AttributeType type);
    int GetAttributeMaxValue(AttributeType type);
    int GetAttributeBaseValue(AttributeType type);
    void UpdateAttributeCurrentValue(AttributeType type, int newValue);

    // TODO: Implementar eventos para notificar a mudança de atributos.

}

