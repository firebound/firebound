namespace DiceRolling.Characters;
/// <summary>
/// Define as ações que um personagem pode realizar.
/// </summary>
public interface ICharacterActionSheet {
    Godot.Collections.Array<CharacterAction> Actions { get; }
    void InitializeActions();
    void AddAction(CharacterAction action);
    void RemoveAction(CharacterAction action);
}
