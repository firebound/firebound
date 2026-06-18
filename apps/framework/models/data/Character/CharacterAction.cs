using DiceRolling.Actions;
using DiceRolling.Roles;
using Godot;

namespace DiceRolling.Characters;

/// <summary>
/// Representa uma ação de um personagem.
/// </summary>
[Tool]
[GlobalClass]
public partial class CharacterAction : Resource {
    [Export] public ActionType? Type { get; set; }

    public CharacterAction() { }

    public CharacterAction(RoleAction roleAction) {
        Type = roleAction.Type;
        if (Type is not null) {
        }
    }

    public void Resolve(IActionContext context) {
        if (Type is null) {
            GD.PrintErr("Action type is null.");
            return;
        }
        foreach (var effect in Type.Effects) {
            effect.Apply(context);
        }
    }
}