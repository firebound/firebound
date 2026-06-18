using Godot;
using DiceRolling.Actions;
using DiceRolling.Attributes;
using DiceRolling.Stores;
using System;
using System.Linq; // For AttributesHelper

namespace DiceRolling.Effects;

/// <summary>
/// An effect that deals damage to the target's Health attribute.
/// </summary>
[Tool]
[GlobalClass]
public partial class DamageEffect : EffectType {

    private string _name = "Effect_" + Guid.NewGuid().ToString("N");

    [Export]
    public string Name {
        get => _name;
        set {
            _name = value;
            EmitChanged();
        }
    }

    [Export(PropertyHint.MultilineText)]
    public string? Description { get; set; }

    [ExportGroup("Configuration")]
    [Export(PropertyHint.Range, "0,100,1")]
    public int DamageValue { get; set; } = 1;

    [Export]
    public string TargetAttributeName { get; set; } = "Health";

    public override void Apply(IActionContext context) {
        if (context?.Target == null) {
            GD.PrintErr("DamageEffect: Target is null in the context.");
            return;
        }

        // Get the AttributeType from the store using the name
        AttributeType? targetAttribute = AttributesStore.Instance.GetAttributeByName(TargetAttributeName);

        if (targetAttribute == null) {
            GD.PrintErr($"DamageEffect ({Name}): TargetAttribute '{TargetAttributeName}' not found in AttributesStore.");
            return;
        }

        var target = context.Target;

        // Check if the target actually has the attribute
        if (!target.Attributes.Any(attr => attr.Type == targetAttribute)) {
            GD.PrintErr($"DamageEffect ({Name}): Target {target.Name} does not possess the attribute {targetAttribute.Name}.");
            return;
        }

        // Get target's current value for the specific attribute
        int currentValue = target.GetAttributeCurrentValue(targetAttribute);

        // Calculate new value
        int newValue = Mathf.Max(targetAttribute.MinValue, currentValue - DamageValue);

        // Apply the new value
        target.UpdateAttributeCurrentValue(targetAttribute, newValue);

        GD.PrintRich($"[color=orange]{context.Attacker?.Name} used {Name ?? "DamageEffect"} on {target.Name}, dealing {DamageValue} {targetAttribute.Name}. {target.Name} {targetAttribute.Name}: {currentValue} -> {newValue}[/color]");

        // TODO: Add visual/audio feedback for damage
        // TODO: Check for target death (if TargetAttribute is Health)
    }

    // Optional: Constructor if needed
    // public DamageEffect() { }
    // public DamageEffect(int damageValue) {
    //     DamageValue = damageValue;
    // }
}
