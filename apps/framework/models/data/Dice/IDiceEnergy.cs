using Godot;

using DiceRolling.Id;

namespace DiceRolling.Dice;

/// <summary>
/// Interface que define as propriedades de uma energia.
/// </summary>
public interface IDiceEnergy : IIdentifiable {

    /// <summary>
    /// Nome da energia.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Descrição da energia.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Cor de fundo da energia.
    /// </summary>
    Color BackgroundColor { get; }

    /// <summary>
    /// Cor principal da energia.
    /// </summary>
    Color MainColor { get; }

    /// <summary>
    /// Ícone da energia.
    /// </summary>
    Texture2D? Icon { get; }

    string? IconPath { get; }
}