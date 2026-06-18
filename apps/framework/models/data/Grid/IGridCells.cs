namespace DiceRolling.Grids;

/// <summary>
/// Interface que define as células de uma grid.
/// </summary>
public interface IGridCells {
    Godot.Collections.Array<GridCellType?>? Cells { get; }
    int GetCellIndex(int row, int column);

    GridCellType? GetCell(int row, int column);

    void SetCellValue(int row, int column, int value);
}