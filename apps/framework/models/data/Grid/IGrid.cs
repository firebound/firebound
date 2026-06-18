using DiceRolling.Id;

namespace DiceRolling.Grids;

/// <summary>
/// Interface que define uma grid completa no jogo.
/// </summary>
public interface IGrid :
    IIdentifiable,
    IGridCells,
    IGridConfiguration {
    void ValidateConstructor();
}