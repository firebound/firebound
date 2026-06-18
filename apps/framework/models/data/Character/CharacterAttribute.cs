using Godot;
using DiceRolling.Attributes;
using DiceRolling.Roles;

namespace DiceRolling.Characters;

/// <summary>
/// Representa um atributo de um personagem.
/// </summary>
[Tool]
[GlobalClass]
public partial class CharacterAttribute : Resource {
    [Export] public AttributeType? Type { get; set; }

    [Export] public int MaxValue { get; set; }

    [Export] public int CurrentValue { get; set; }

    [Export] public int BaseValue { get; set; }

    public CharacterAttribute() { }

    public CharacterAttribute(RoleAttribute roleAttribute) {
        Type = roleAttribute.Type;
        BaseValue = roleAttribute.BaseValue;

        MaxValue = BaseValue;
        CurrentValue = BaseValue;
    }

    public override void _ValidateProperty(Godot.Collections.Dictionary property) {
        if (property["name"].AsStringName() == "BaseValue") {
            property["usage"] = (int)(PropertyUsageFlags.Default | PropertyUsageFlags.ReadOnly);
        }
        base._ValidateProperty(property);
    }
}