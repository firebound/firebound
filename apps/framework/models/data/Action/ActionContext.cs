using DiceRolling.Characters;

namespace DiceRolling.Actions;

/// <summary>
/// Concrete implementation of IActionContext, holding the attacker and target for an action execution.
/// </summary>
public class ActionContext : IActionContext {
    public CharacterType Attacker { get; }
    public CharacterType? Target { get; }

    public ActionContext(CharacterType attacker, CharacterType? target) {
        Attacker = attacker;
        Target = target;
    }
}