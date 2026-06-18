using Godot;
using DiceRolling.Actions;

namespace DiceRolling.Effects;

[Tool]
public abstract partial class EffectType : Resource, IEffect {
    public abstract void Apply(IActionContext context);
}