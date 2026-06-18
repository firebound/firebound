using DiceRolling.Id;

namespace DiceRolling.Attributes;

/// <summary>
/// Interface que define um atributo completo no jogo.
/// </summary>
public interface IAttribute :
    IIdentifiable,
    IAttributeInformation,
    IAttributeAssets,
    IAttributeValues {

    /// <summary>
    /// Valida os campos do resource.
    /// </summary>
    void ValidateConstructor();
}