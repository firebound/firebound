namespace DiceRolling.Locations;

/// <summary>
/// Interface que define uma localização no jogo.
/// </summary>
public interface ILocation {
    /// <summary>
    /// Nome da localização.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Descrição da localização.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Número total de slots na localização.
    /// </summary>
    int TotalSlots { get; }
}