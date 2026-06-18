using DiceRolling.Actions;

namespace DiceRolling.Effects;

/// <summary>
/// Interface que define um efeito no jogo.
/// </summary>
public interface IEffect {
    /// <summary>
    /// Aplica o efeito no contexto da ação.
    /// </summary>
    /// <param name="context">O contexto da ação.</param>
    void Apply(IActionContext context);
}