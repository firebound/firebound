using Godot;
using System.Collections.Generic;
using System.Linq;
using DiceRolling.Grids;
using DiceRolling.Targets;

namespace DiceRolling.Editor;

/// <summary>
/// Manages a grid configuration for a target.
/// </summary>
[Tool]
public partial class BoardMatrixEditor : Control {
    private bool isFlippedHorizontally = false;
    private const int CellSize = 40;
    private const int Padding = 10;
    private static readonly Color[] ColorsArray = { Colors.White, Colors.Yellow, Colors.Green, Colors.Red };
    private readonly List<GridType> _grids = [];
    public TargetBoardType? TargetBoard { get; }

    public BoardMatrixEditor() {
        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SizeFlagsVertical = SizeFlags.ExpandFill;
        CustomMinimumSize = new Vector2(0, 0);
        UpdateMinimumSize();
    }

    public BoardMatrixEditor(TargetBoardType targetBoard) : this() {
        TargetBoard = targetBoard;
        ClearGrids();
        foreach (var grid in targetBoard.Grids) {
            AddGrid(grid);
        }
        UpdateMinimumSize();
    }

    public void AddGrid(GridType grid) {
        _grids.Add(grid);
        if (!grid.IsConnected(nameof(GridType.GridChanged), new Callable(this, nameof(OnGridConfigurationChanged)))) {
            grid.Connect(nameof(GridType.GridChanged), new Callable(this, nameof(OnGridConfigurationChanged)));
        }
        UpdateMinimumSize();
    }

    public void ClearGrids() {
        foreach (var grid in _grids) {
            grid.Disconnect(nameof(GridType.GridChanged), new Callable(this, nameof(OnGridConfigurationChanged)));
        }
        _grids.Clear();
        UpdateMinimumSize();
        QueueRedraw();
    }

    public void ClearGridInputs() {
        foreach (var grid in _grids) {
            for (int y = 0; y < grid.Rows; y++) {
                for (int x = 0; x < grid.Columns; x++) {
                    grid.SetCellValue(y, x, 0);
                }
            }
        }
        QueueRedraw();
    }

    private new void UpdateMinimumSize() {
        int maxColumns = _grids.Count > 0 ? _grids.Max(g => g?.Columns ?? 0) : 0;
        int maxRows = _grids.Count > 0 ? _grids.Max(g => g?.Rows ?? 0) : 0;
        CustomMinimumSize = new Vector2(maxColumns * CellSize + Padding * 2, maxRows * CellSize + Padding * 2);
    }

    public void FlipHorizontally(bool flip) {
        isFlippedHorizontally = flip;
        _grids.Reverse();
        QueueRedraw();
    }

    public override void _Draw() {
        float offsetX = 0;
        foreach (var grid in _grids) {
            if (grid != null) {
                DrawGrid(grid, offsetX);
                offsetX += grid.Columns * CellSize + Padding;
            }
        }
    }

    private void DrawGrid(GridType grid, float offsetX) {
        GD.Print($"Drawing grid with Rows: {grid.Rows}, Columns: {grid.Columns}");
        if (grid.Rows <= 0 || grid.Columns <= 0) {
            GD.PrintErr($"Invalid grid configuration: Rows = {grid.Rows}, Columns = {grid.Columns}");
            return;
        }

        var offsetY = Padding;

        for (int y = 0; y < grid.Rows; y++) {
            for (int x = 0; x < grid.Columns; x++) {
                int drawX = isFlippedHorizontally ? grid.Columns - 1 - x : x;
                var rect = new Rect2(drawX * CellSize + Padding + offsetX, y * CellSize + Padding + offsetY, CellSize, CellSize);

                // Get GridCellType and extract its value
                var cellData = grid.GetCell(y, x);
                int value = cellData?.Value ?? 0;

                Color bgColor = ColorsArray[Mathf.Clamp(value, 0, ColorsArray.Length - 1)];
                DrawRect(rect, bgColor);
                DrawRect(rect, Colors.Black, false);
                string text = $"{grid.Prefix}{grid.GetCellIndex(y, x)} [{value}]";
                DrawString(GetThemeFont("font"), rect.Position + new Vector2(5, 15), text, HorizontalAlignment.Left, -1, 12, Colors.Black);
            }
        }
    }

    public override void _GuiInput(InputEvent @event) {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
            float offsetX = 0;
            foreach (var grid in _grids) {
                if (HandleMouseInput(mouseEvent, grid, offsetX)) {
                    break;
                }
                offsetX += grid.Columns * CellSize + Padding;
            }
        }
    }

    private bool HandleMouseInput(InputEventMouseButton mouseEvent, GridType grid, float offsetX) {
        var offsetY = Padding;

        var x = (int)((mouseEvent.Position.X - Padding - offsetX) / CellSize);
        var y = (int)((mouseEvent.Position.Y - Padding - offsetY) / CellSize);

        if (isFlippedHorizontally) {
            x = grid.Columns - 1 - x;
        }

        if (x >= 0 && x < grid.Columns && y >= 0 && y < grid.Rows) {
            // Get GridCellType and extract its value
            var cellData = grid.GetCell(y, x);
            int value = cellData?.Value ?? 0;

            if (mouseEvent.ButtonIndex == MouseButton.Left) {
                grid.SetCellValue(y, x, (value + 1) % ColorsArray.Length);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right) {
                grid.SetCellValue(y, x, (value - 1 + ColorsArray.Length) % ColorsArray.Length);
            }
            QueueRedraw();
            return true;
        }
        return false;
    }

    private void OnGridConfigurationChanged() {
        UpdateMinimumSize();
        QueueRedraw();
    }
}