using Godot;
using System.Linq;
using DiceRolling.Dice;
using DiceRolling.Stores;

namespace DiceRolling.Helpers;

public static class DiceEnergyHelper {
    public static DiceEnergy? GetEnergyByName(DiceEnergyStore store, string energyName) {
        var energy = store.Energy.FirstOrDefault(e => e.Name == energyName);
        if (energy == null) {
            GD.PrintErr($"DiceEnergy '{energyName}' not found in DiceEnergyStore.");
        }
        return energy;
    }

    public static DiceEnergy? GetEnergyById(DiceEnergyStore store, string energyId) {
        var energy = store.Energy.FirstOrDefault(e => e.Id == energyId);
        if (energy == null) {
            GD.PrintErr($"DiceEnergy with ID '{energyId}' not found in DiceEnergyStore.");
        }
        return energy;
    }
}