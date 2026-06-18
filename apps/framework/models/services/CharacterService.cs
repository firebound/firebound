using Godot;
using System.Linq;
using System;

using DiceRolling.Attributes;
using DiceRolling.Characters;

namespace DiceRolling.Services;

/// <summary>
/// Fornece métodos para manipulação dos dados de personagens.
/// </summary>
public class CharacterService {
    private static CharacterService? _instance;
    public static CharacterService Instance => _instance ??= new CharacterService();

    public virtual void InitializeAttributes(CharacterType character) {
        if (character.Role is null) {
            GD.PrintErr("Role is null");
            return;
        }

        if (character.Role is null || character.Role.RoleAttributes.Count == 0) {
            GD.PrintErr("RoleAttributes is empty");
            return;
        }

        if (character.Attributes.Count == 0) {
            foreach (var roleAttribute in character.Role.RoleAttributes) {
                var characterAttribute = new CharacterAttribute(roleAttribute) {
                    MaxValue = roleAttribute.BaseValue,
                    CurrentValue = roleAttribute.BaseValue
                };
                character.Attributes.Add(characterAttribute);
            }
        }
    }

    public virtual void InitializeActions(CharacterType character) {
        if (character.Role is null) {
            GD.PrintErr("Role is null");
            return;
        }

        if (character.Role is null || character.Role.RoleActions.Count == 0) {
            GD.PrintErr("RoleActions is empty");
            return;
        }

        if (character.Actions.Count == 0) {
            foreach (var roleAction in character.Role.RoleActions) {
                var characterAction = new CharacterAction(roleAction);
                character.Actions.Add(characterAction);
            }
        }
    }

    public static int GetAttributeCurrentValue(CharacterType character, AttributeType type) {
        var attribute = character.Attributes.FirstOrDefault(attr => attr.Type == type);
        return attribute is not null ? attribute.CurrentValue : 0;
    }

    public static int GetAttributeMaxValue(CharacterType character, AttributeType type) {
        var attribute = character.Attributes.FirstOrDefault(attr => attr.Type == type);
        return attribute is not null ? attribute.MaxValue : 0;
    }

    public static int GetAttributeBaseValue(CharacterType character, AttributeType type) {
        var attribute = character.Attributes.FirstOrDefault(attr => attr.Type == type);
        return attribute is not null ? attribute.BaseValue : 0;
    }

    public static void UpdateAttributeCurrentValue(CharacterType character, AttributeType type, int newValue) {
        var attribute = character.Attributes.FirstOrDefault(attr => attr.Type == type);
        if (attribute is not null) {
            attribute.CurrentValue = newValue;
            // EmitSignal(nameof(AttributeChanged), character, type);
        }
    }

    public static void AddAction(CharacterType character, CharacterAction action) {
        ArgumentNullException.ThrowIfNull(action);
        character.Actions.Add(action);
    }

    public static void RemoveAction(CharacterType character, CharacterAction action) {
        ArgumentNullException.ThrowIfNull(action);
        character.Actions.Remove(action);
    }
}