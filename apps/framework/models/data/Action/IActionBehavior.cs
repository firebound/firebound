using Godot.Collections;
using DiceRolling.Dice;
using DiceRolling.Targets;
using DiceRolling.Effects;

namespace DiceRolling.Actions;

/// <summary>
/// Interface que define o comportamento de uma ação.
/// </summary>
public interface IActionBehavior<TContext, TResult> {
    Array<DiceEnergy> RequiredEnergy { get; set; }
    Array<EffectType> Effects { get; set; }
    TargetBoardType? TargetBoard { get; set; }
    TResult Do(TContext context);
}