using DiceRolling.Locations;

namespace DiceRolling.Characters;

/// <summary>
/// Define a localização de um personagem no jogo.
/// </summary>
public interface ICharacterPlacementSheet {
    LocationType? Location { get; set; }
    int SlotIndex { get; set; }
}