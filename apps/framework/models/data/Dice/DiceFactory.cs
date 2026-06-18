using DiceRolling.Stores;
namespace DiceRolling.Dice;

public static class DiceFactory {
    public static DiceType CreateD4(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(4, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD6(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(6, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD8(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(8, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD10(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(10, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD12(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(12, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD20(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(20, DiceEnergyConfig, locationCategory, characterId);
    public static DiceType CreateD100(DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) => CreateDice(100, DiceEnergyConfig, locationCategory, characterId);

    private static DiceType CreateDice(int sides, DiceEnergyStore DiceEnergyConfig, DiceLocationCategory locationCategory, string? characterId = null) {
        var energies = new Godot.Collections.Array<DiceSide>();
        var diceEnergies = DiceEnergyConfig.Energy;
        for (int i = 0; i < sides; i++) {
            var energy = diceEnergies[i % diceEnergies.Count];
            energies.Add(new DiceSide(energy));
        }
        var location = new DiceLocation(locationCategory, characterId);
        return new DiceType($"D{sides}", energies, location);
    }
}