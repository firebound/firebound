using Godot;

using DiceRolling.Dice;

namespace DiceRolling.Stores;

[Tool]
[GlobalClass]
public partial class DiceEnergyStore : Resource {
    [Export]
    public Godot.Collections.Array<DiceEnergy> Energy { get; set; } = [];

    public DiceEnergyStore() { }
}