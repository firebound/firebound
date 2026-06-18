using Godot;
using DiceRolling.Id;

namespace DiceRolling.Dice;

/// <summary>
/// Interface que define um dado completo no jogo.
/// </summary>
/// <typeparam name="T">Tipo de lado do dado.</typeparam>
public interface IDice<[MustBeVariant] T> : IIdentifiable where T : DiceSide {
    string Name { get; }
    int SideCount { get; }
    Godot.Collections.Array<T> Sides { get; }
    DiceLocation Location { get; }
    void ValidateConstructor();
}