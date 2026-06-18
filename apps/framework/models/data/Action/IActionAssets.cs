using Godot;

namespace DiceRolling.Actions;

/// <summary>
/// Interface que define os recursos visuais de uma ação.
/// </summary>
public interface IActionAssets {
    Texture2D? Icon { get; set; }
    string? IconPath { get; }
}