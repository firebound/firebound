namespace DiceRolling.Grids;

/// <summary>
/// Interface que define a configuração de uma grid.
/// </summary>
public interface IGridConfiguration {
    int Rows { get; }

    int Columns { get; }

    string Prefix { get; }

    GridDirection Direction { get; }
}