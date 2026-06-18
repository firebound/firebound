using DiceRolling.Actions;
using DiceRolling.Characters;

namespace DiceRolling.Controllers;

/// <summary>
/// Stores information about an action declared by a character, including the action itself and the chosen target.
/// </summary>
public class DeclaredActionInfo {
    public ActionType? Action { get; }
    public CharacterType? Target { get; }

    // Constructor for a declared action with a target
    public DeclaredActionInfo(ActionType action, CharacterType target) {
        Action = action;
        Target = target;
    }

    // Constructor for passing (no action declared)
    public DeclaredActionInfo() {
        Action = null;
        Target = null;
    }

    public bool IsPass() => Action == null;
}