using Godot;

namespace DiceRolling.Attributes;

/// <summary>
/// Interface que define os recursos visuais de um atributo.
/// </summary>
public interface IAttributeAssets {
    /// <summary>
    /// Cor do atributo.
    /// </summary>
    Color Color { get; set; }

    /// <summary>
    /// Ícone do atributo.
    /// </summary>
    Texture2D? Icon { get; set; }

    /// <summary>
    /// Caminho do ícone do atributo.
    /// </summary>
    string? IconPath { get; }
}