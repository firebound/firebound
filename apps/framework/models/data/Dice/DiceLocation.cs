using Godot;

namespace DiceRolling.Dice;

public partial class DiceLocation : Resource, IDiceLocation {
    [Export] public DiceLocationCategory LocationCategory { get; set; }
    [Export] public string? CharacterId { get; set; }

    public DiceLocation() {
        LocationCategory = DiceLocationCategory.None;
        CharacterId = null;
    }

    public DiceLocation(DiceLocationCategory locationCategory, string? characterId = null) {
        LocationCategory = locationCategory;
        CharacterId = characterId;
    }
}