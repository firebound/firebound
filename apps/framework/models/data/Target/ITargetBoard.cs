using Godot;
using DiceRolling.Grids;

namespace DiceRolling.Targets;

/// <summary>
/// Interface que define a configuração de um tabuleiro de alvos no jogo.
/// </summary>
public interface ITargetBoard {

    /// <summary>
    /// Evento emitido quando a configuração do tabuleiro é alterada.
    /// </summary>
    [Signal] delegate void ConfigurationChangedEventHandler();

    /// <summary>
    /// Indica se o tabuleiro é para um único alvo.
    /// </summary>
    bool IsSingleTarget { get; set; }

    /// <summary>
    /// Coleção de grids associadas ao tabuleiro.
    /// </summary>
    Godot.Collections.Array<GridType> Grids { get; set; }

    /// <summary>
    /// Adiciona uma nova grid ao tabuleiro.
    /// </summary>
    /// <param name="rows">Número de linhas da grid.</param>
    /// <param name="columns">Número de colunas da grid.</param>
    void AddGrid(int rows, int columns);

    /// <summary>
    /// Atualiza uma grid existente no tabuleiro.
    /// </summary>
    /// <param name="index">Índice da grid a ser atualizada.</param>
    void UpdateGrid(int index);
}