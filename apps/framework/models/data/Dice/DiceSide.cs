using Godot;

namespace DiceRolling.Dice;

/// <summary>
/// Represents a side of a dice with a specific type of energy.
/// </summary>
[Tool]
[GlobalClass]
public partial class DiceSide : Resource {

    [Export]
    public DiceEnergy? Energy { get; set; }


    public DiceSide() {
    }

    public DiceSide(DiceEnergy energy) {
        Energy = energy;
    }
}