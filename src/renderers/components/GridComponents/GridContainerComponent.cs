using Godot;
using System.Collections.Generic;
using DiceRolling.Entities;
using DiceRolling.Grids;

namespace DiceRolling.Components.Grids;

/// <summary>
/// A component that creates a grid of cells based on a GridEntity's GridData.
/// </summary>
/// <remarks>
/// This component listens for updates to the parent GridEntity's GridData and rebuilds the grid of cells accordingly.
/// </remarks>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-3d.svg")]
public partial class GridContainerComponent : Node3D {
    private GridEntity? _parent;

    [Export] public PackedScene? CellScene { get; set; }
    [Export] public Vector3 CellSize { get; set; } = new Vector3(1, 0, 1);
    [Export] public Vector3 GridOffset { get; set; } = Vector3.Zero;

    private readonly Dictionary<string, GridCellEntity> _cellEntities = [];

    public override void _Ready() {
        _parent = GetParent() as GridEntity;
        if (_parent != null) {
            _parent.Connect(Entity3D.SignalName.EntityUpdated, Callable.From(RebuildGrid));
            RebuildGrid();
        }
    }

    private void RebuildGrid() {
        ClearGrid();
        if (_parent?.GridData?.Cells == null || CellScene == null) return;

        foreach (var cell in _parent.GridData.Cells) {
            if (cell == null) continue;

            CreateCellEntity(cell);
        }
    }

    private void ClearGrid() {
        foreach (var entity in _cellEntities.Values) {
            entity.QueueFree();
        }
        _cellEntities.Clear();
    }

    private GridCellEntity CreateCellEntity(GridCellType cell) {
        if (CellScene == null) {
            GD.PrintErr("Cannot create cell entity: CellScene is null.");
            throw new System.ArgumentNullException(nameof(CellScene), "CellScene must be assigned to create grid cell entities.");
        }

        var instance = CellScene.Instantiate<GridCellEntity>();
        instance.Data = cell;
        instance.Position = CalculateCellPosition(cell);
        AddChild(instance);
        _cellEntities[cell.Id] = instance;
        return instance;
    }

    private Vector3 CalculateCellPosition(GridCellType cell) {
        return new Vector3(
            cell.Column * CellSize.X + GridOffset.X + CellSize.X / 2,
            GridOffset.Y,
            cell.Row * CellSize.Z + GridOffset.Z + CellSize.Z / 2
        );
    }
}