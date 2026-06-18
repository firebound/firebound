using Godot;
using DiceRolling.Actions;
using DiceRolling.Characters;
using DiceRolling.Dice;
using DiceRolling.Grids;
using DiceRolling.Targets;
using DiceRolling.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace DiceRolling.Services;

/// <summary>
/// Provides helper methods for managing and validating character actions.
/// </summary>
public static class ActionService {

    /// <summary>
    /// Checks if a character has enough available energy to pay the cost of an action.
    /// </summary>
    /// <param name="character">The character attempting the action.</param>
    /// <param name="action">The action being attempted.</param>
    /// <returns>True if the character can afford the action, false otherwise.</returns>
    public static bool CanAffordAction(CharacterType character, ActionType action) {
        if (character == null || action == null) {
            GD.PrintErr("ActionService.CanAffordAction: Character or Action is null.");
            return false;
        }

        // If no energy is required, the action is always affordable.
        if (action.RequiredEnergy == null || action.RequiredEnergy.Count == 0) {
            return true;
        }

        // If energy is required but the character has none available, they cannot afford it.
        if (character.AvailableEnergy == null || character.AvailableEnergy.Count == 0) {
            GD.PrintRich($"[color=yellow]{character.Name} has no available energy (AvailableEnergy: {character.AvailableEnergy?.Count ?? 0})[/color]");
            return false;
        }

        // Create a mutable copy of the available energy to simulate spending.
        // Using List<T> for easier removal.
        var availableEnergyCopy = new List<DiceEnergy>(character.AvailableEnergy);

        // Try to "pay" each required energy cost.
        foreach (var required in action.RequiredEnergy) {
            if (required == null) continue; // Skip null requirements

            bool foundAndRemoved = false;
            for (int i = 0; i < availableEnergyCopy.Count; i++) {
                // Assuming DiceEnergy resources are unique and can be compared by reference
                // or have overridden Equals/GetHashCode if they represent types.
                if (availableEnergyCopy[i].Id == required.Id) { // semantically compare by energy type
                    availableEnergyCopy.RemoveAt(i); // Consume the energy
                    foundAndRemoved = true;
                    break; // Move to the next required energy
                }
            }
            // If a required energy type couldn't be found in the available pool, the character cannot afford the action.
            if (!foundAndRemoved) {
                GD.PrintRich($"[color=yellow]{character.Name} cannot afford {action.Name}: Missing {required.Name}. Available energies: [{string.Join(", ", character.AvailableEnergy.Select(e => e?.Name ?? "null"))}][/color]");
                return false;
            }
        }

        // If all required energy costs were successfully found and removed, the character can afford the action.
        return true;
    }

    /// <summary>
    /// Determines the valid targets for a given action based on the attacker's position and the action's target configuration.
    /// </summary>
    /// <param name="attacker">The character performing the action.</param>
    /// <param name="action">The action being performed.</param>
    /// <param name="potentialTargets">A list of all characters who could potentially be targeted.</param>
    /// <returns>A list of characters who are valid targets for the action.</returns>
    public static List<CharacterType> GetValidTargets(CharacterType attacker, ActionType action, List<CharacterType> potentialTargets) {
        var validTargets = new List<CharacterType>();

        if (attacker == null || action == null || potentialTargets == null) {
            GD.PrintErr("ActionService.GetValidTargets: Attacker, Action, or PotentialTargets list is null.");
            return validTargets;
        }

        TargetBoardType? targetBoard = action.TargetBoard;

        // Check if the TargetBoard is configured for positional targeting (needs at least 2 grids)
        if (targetBoard == null || targetBoard.Grids == null || targetBoard.Grids.Count < 2) {
            // TODO: Handle non-positional actions (e.g., self-target, target all enemies/allies)
            // For now, assume invalid if board isn't set up for attacker/target grids.
            GD.Print($"Action {action.Name} does not use standard positional targeting or TargetBoard is invalid.");
            return validTargets; // Return empty list for now
        }

        GridType actorGrid = targetBoard.Grids[0];
        GridType targetGrid = targetBoard.Grids[1];

        if (actorGrid == null || targetGrid == null) {
            GD.PrintErr($"Action {action.Name} has null grids in its TargetBoard.");
            return validTargets;
        }

        // Determine if the grid perspective needs to be flipped (e.g., for enemies)
        bool needsFlip = attacker.Location == BattleController.Instance.EnemySquadLocation;

        // 1. Check if the attacker is in a valid position according to the actorGrid (value 1)
        int attackerCellValue = actorGrid.GetCellValueAt(attacker.SlotIndex, needsFlip);
        if (attackerCellValue != 1) { // 1 = Actor Placement
            // GD.Print($"{attacker.Name} cannot use {action.Name} from slot {attacker.SlotIndex} (needs cell value 1, found {attackerCellValue})");
            return validTargets; // Attacker is not in a valid position to use this action.
        }

        // 2. Iterate through potential targets to see if they are in valid positions
        foreach (var target in potentialTargets) {
            // Basic checks: target exists, is not the attacker (unless action allows self-target - handle later)
            if (target == null || target == attacker) continue;

            // Check if the target's position matches a target cell (value 3) in the targetGrid
            int targetCellValue = targetGrid.GetCellValueAt(target.SlotIndex, needsFlip); // Use the same flip status based on the attacker's perspective
            if (targetCellValue == 3) { // 3 = Target
                validTargets.Add(target);
            }
        }

        return validTargets;
    }

    /// <summary>
    /// Consumes the required energy for an action from a character's available energy pool.
    /// Assumes CanAffordAction was already checked and returned true.
    /// </summary>
    /// <param name="character">The character spending the energy.</param>
    /// <param name="action">The action whose energy cost is being spent.</param>
    public static void ConsumeEnergy(CharacterType character, ActionType action) {
        if (character == null || action == null || action.RequiredEnergy == null || action.RequiredEnergy.Count == 0) {
            return; // Nothing to consume
        }

        if (character.AvailableEnergy == null) {
            GD.PrintErr($"ActionService.ConsumeEnergy: Character {character.Name} has null AvailableEnergy.");
            return;
        }

        // It's generally safer to modify the collection directly if it's designed for it.
        // Godot.Collections.Array allows Remove().
        foreach (var required in action.RequiredEnergy) {
            if (required == null) continue;

            bool removed = character.AvailableEnergy.Remove(required); // Remove the first matching energy
            if (!removed) {
                // This should ideally not happen if CanAffordAction passed, but log if it does.
                GD.PrintErr($"ActionService.ConsumeEnergy: Failed to remove required energy {required.Name} from {character.Name}. Available: [{string.Join(", ", character.AvailableEnergy.Select(e => e?.Name ?? "null"))}]");
            }
        }
        // Optionally, emit a signal that the character's energy changed
        // character.EmitSignal(nameof(CharacterType.EnergyChanged));
    }
}