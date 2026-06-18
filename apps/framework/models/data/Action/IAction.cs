using DiceRolling.Id;
using DiceRolling.Effects;

namespace DiceRolling.Actions;

/// <summary>
/// Interface que define as entidades de ações que são realizadas por personagens.
/// </summary>
public interface IAction<TContext, TResult> :
    IIdentifiable,
    IActionInformation,
    IActionAssets,
    IActionBehavior<TContext, TResult> {

    bool IsValid();
    void AddEffect(EffectType effect);
    void RemoveEffect(EffectType effect);
    void ValidateConstructor();
}