using DiceRolling.Categories;
using DiceRolling.Roles;

namespace DiceRolling.Characters;

/// <summary>
/// Define informações gerais de um personagem.
/// </summary>
public interface ICharacterInformationSheet {
    string Name { get; set; }
    Category? Category { get; set; }
    RoleType? Role { get; set; }
}