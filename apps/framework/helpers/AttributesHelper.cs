using Godot;
using DiceRolling.Attributes;
using DiceRolling.Stores;
using System.Linq;

namespace DiceRolling.Helpers;

public static class AttributesHelper {
    public static AttributeType? GetAttributeType(AttributesStore config, string attributeName) {
        if (string.IsNullOrWhiteSpace(attributeName) || config == null) {
            GD.PrintErr("AttributesHelper.GetAttributeType: Invalid parameters (config or attributeName is null/whitespace).");
            return null;
        }

        var attribute = config.Attributes.FirstOrDefault(attr => attr.Name.Equals(attributeName, System.StringComparison.OrdinalIgnoreCase));

        if (attribute == null) {
            GD.PrintErr($"Attribute '{attributeName}' not found in AttributesStore.");
        }
        return attribute;
    }
}