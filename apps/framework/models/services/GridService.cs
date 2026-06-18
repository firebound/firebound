using System;
using Godot;
using DiceRolling.Grids;

namespace DiceRolling.Services;

public class GridService {
    private static GridService? _instance;
    public static GridService Instance => _instance ??= new GridService();

    private GridService() { }

    public static int GetCellIndex(int row, int column, int columns) {
        return row * columns + column;
    }

    public static GridCellType CreateCellType(int value, int row, int column, int columns, string prefix = "") {
        int index = GetCellIndex(row, column, columns);
        return new GridCellType {
            Value = value,
            Row = row,
            Column = column,
            Index = index,
            Label = string.IsNullOrEmpty(prefix) ? $"({row},{column})" : $"{prefix}{index}"
        };
    }

    public static void ResizeGridCells(Godot.Collections.Array<GridCellType?>? cells, int rows, int columns, string prefix = "") {
        ArgumentNullException.ThrowIfNull(cells);

        int requiredSize = rows * columns;
        int currentSize = cells.Count;

        if (requiredSize > currentSize) {
            // Add new cells
            for (int i = currentSize; i < requiredSize; i++) {
                int row = i / columns;
                int col = i % columns;
                cells.Add(CreateCellType(0, row, col, columns, prefix));
            }
        }
        else if (requiredSize < currentSize) {
            // Remove excess cells
            cells.Resize(requiredSize);
        }

        // Update existing cells' positions
        for (int i = 0; i < requiredSize; i++) {
            var cell = cells[i];
            if (cell != null) {
                int row = i / columns;
                int col = i % columns;
                cell.Row = row;
                cell.Column = col;
                cell.Index = i;
                cell.Label = $"{prefix}{i}";
            }
        }
    }

    public static GridCellType? GetGridCell(Godot.Collections.Array<GridCellType?>? cells, int row, int column, int columns) {
        if (cells == null) return null;

        int index = GetCellIndex(row, column, columns);
        return (index >= 0 && index < cells.Count) ? cells[index] : null;
    }

    public static void SetGridCellValue(Godot.Collections.Array<GridCellType?>? cells, int row, int column, int value, int columns) {
        if (cells == null) return;

        if (row < 0 || column < 0 || row >= (cells.Count / columns) || column >= columns) {
            GD.PrintErr("Invalid cell position");
            return;
        }

        int index = GetCellIndex(row, column, columns);
        if (index >= 0 && index < cells.Count) {
            var cell = cells[index];
            if (cell != null) {
                cell.Value = value;
                cell.NotifyChanged();
            }
        }
    }

    public static void AssignCharactersToGrid(GridType grid) {
        if (grid.CharacterStore == null) {
            GD.PrintErr("Cannot assign characters: CharacterStore is null");
            return;
        }

        if (grid.Cells == null) {
            GD.PrintErr("Cannot assign characters: Grid Cells collection is null");
            return;
        }

        // Clear all character references first
        foreach (var cell in grid.Cells) {
            if (cell != null) {
                cell.Character = null;
                cell.IsOccupied = false;
            }
        }

        // Get all characters from the store
        foreach (var character in grid.CharacterStore.Characters) {
            // TODO - Check if character is in the right location for this grid
            if (character.Location != null && character.SlotIndex >= 0) {
                // Map the slot index to a cell in the grid
                int row = character.SlotIndex / grid.Columns;
                int column = character.SlotIndex % grid.Columns;

                // Check if the cell position is valid
                if (row < grid.Rows && column < grid.Columns) {
                    var cell = grid.GetCell(row, column);
                    if (cell != null) {
                        cell.Character = character;
                        cell.IsOccupied = true;
                        cell.NotifyChanged();
                    }
                }
            }
        }

        grid.EmitSignal(nameof(grid.GridChanged));
    }
}