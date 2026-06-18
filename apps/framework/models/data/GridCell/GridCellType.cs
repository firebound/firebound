using Godot;
using DiceRolling.Id;
using DiceRolling.Characters;

namespace DiceRolling.Grids;

[Tool]
[GlobalClass]
public partial class GridCellType : IdentifiableResource {
    [Export] public CharacterType? Character { get; set; }
    [Export] public int Value { get; set; }
    [Export] public int Row { get; set; }
    [Export] public int Column { get; set; }
    [Export] public int Index { get; set; }
    [Export] public string Label { get; set; } = string.Empty;
    [Export] public bool IsWalkable { get; set; } = true;
    [Export] public bool IsOccupied { get; set; }
    [Signal] public delegate void CellChangedEventHandler();

    public void NotifyChanged() {
        EmitSignal(SignalName.CellChanged);

        EmitChanged();
    }
}