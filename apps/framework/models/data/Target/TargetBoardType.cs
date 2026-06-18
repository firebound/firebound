using Godot;
using System;

using DiceRolling.Grids;

namespace DiceRolling.Targets;

[Tool]
[GlobalClass]
public partial class TargetBoardType : Resource, ITargetBoard {
    [Signal] public delegate void SetupChangedEventHandler();

    [Export]
    public bool IsSingleTarget { get; set; } = false;

    [Export]
    public Godot.Collections.Array<GridType> Grids { get; set; } = [];

    public TargetBoardType() { }

    public void AddGrid(int rows, int columns) {
        if (rows <= 0 || columns <= 0) {
            throw new ArgumentException("Rows e Columns devem ser maiores que 0.");
        }
        Grids.Add(new GridType(rows, columns, "G"));
        EmitSignal(nameof(SetupChanged));
    }

    public void UpdateGrid(int index) {
        if (index >= 0 && index < Grids.Count) {
            Grids[index].ResizeCells();
            EmitSignal(nameof(SetupChanged));
        }
    }
}