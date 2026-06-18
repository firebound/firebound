namespace DiceRolling.Dice;

/// <summary>
/// Interface que define a localização de um dado.
/// </summary>
public interface IDiceLocation {
    /// <summary>
    /// Categoria de localização do dado.
    /// </summary>
    DiceLocationCategory LocationCategory { get; }

    /// <summary>
    /// ID do personagem, se estiver com um personagem.
    /// </summary>
    string? CharacterId { get; }
}