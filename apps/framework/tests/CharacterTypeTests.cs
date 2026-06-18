using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using DiceRolling.Characters;
using DiceRolling.Roles;
using DiceRolling.Attributes;
using DiceRolling.Helpers;
using DiceRolling.Actions;
using DiceRolling.Stores;

namespace DiceRolling.Tests.Characters;

[TestSuite]
public class CharacterTypeTests {
    private AttributesStore? _AttributesStore;

    [Before]
    public void SetUp() {
        // TODO - achar um jeito melhor de acessar a coleção de atributos do jogo
        _AttributesStore = GD.Load<AttributesStore>("res://resources/Attributes/AttributesStore.tres");
    }

    [TestCase]
    public void Constructor_ShouldInitializeAttributesAndActions() {
        // Arrange
        if (_AttributesStore == null) {
            AssertThat(false).IsTrue();
            GD.PrintErr("AttributesStore is null");
            return;
        }

        // Create a valid ActionType first
        var actionType = new ActionType {
            Name = "Attack",
            Description = "Basic attack",
        };

        // Create the role with complete initialization
        var role = new RoleType(
                "Warrior",
                "A brave warrior",
                [
                    new RoleAttribute {
                        Type = AttributesHelper.GetAttributeType(_AttributesStore, "Health")!,
                        BaseValue = 10,
                    },
                ],
                [
                    new RoleAction {
                        Type = actionType,
                    },
                ]
            );

        // Ensure all objects are not null
        if (role == null) {
            AssertThat(false).IsTrue();
            GD.PrintErr("Role is null");
            return;
        }

        // Act
        var character = new CharacterType("Hero", role);

        // Assert
        AssertThat(character.Name).IsEqual("Hero");
        AssertThat(character.Attributes.Count).IsEqual(1);
        AssertThat(character.Attributes[0].Type?.Name).IsEqual("Health");
        AssertThat(character.Attributes[0].BaseValue).IsEqual(10);
        AssertThat(character.Actions.Count).IsEqual(1);
        AssertThat(character.Actions[0].Type?.Name).IsEqual("Attack");
    }
}