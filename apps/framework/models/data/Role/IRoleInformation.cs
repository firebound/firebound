namespace DiceRolling.Roles;

/// <summary>
/// Interface que define as informações básicas de um arquétipo de personagem no jogo.
/// </summary>
public interface IRoleInformation {

    /// <summary>
    /// Nome do arquétipo de personagem.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Descrição do arquétipo de personagem.
    /// </summary>
    string? Description { get; set; }
}