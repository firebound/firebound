namespace DiceRolling.Attributes;

/// <summary>
/// Interface que define os valores de um atributo.
/// </summary>
public interface IAttributeValues {
    /// <summary>
    /// Valor mínimo do atributo.
    /// </summary>
    int MinValue { get; }

    /// <summary>
    /// Valor máximo do atributo.
    /// </summary>
    int MaxValue { get; }
}