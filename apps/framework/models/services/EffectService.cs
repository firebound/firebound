using DiceRolling.Actions;
using DiceRolling.Effects;

namespace DiceRolling.Services;

public static class EffectService {
    public static void ApplyEffects(Godot.Collections.Array<EffectType> effects, IActionContext context) {
        foreach (var effect in effects) {
            effect.Apply(context);
        }
    }
}