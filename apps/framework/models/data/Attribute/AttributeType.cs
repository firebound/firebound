using Godot;
using System;
using DiceRolling.Id;
using DiceRolling.Services;

namespace DiceRolling.Attributes;

[Tool]
[GlobalClass]
public partial class AttributeType : IdentifiableResource, IAttribute {
    private string _name = "Attribute" + Guid.NewGuid().ToString("N");
    private Texture2D? _icon;
    private int _minValue = 0;
    private int _maxValue = 1;

    [ExportGroup("📝 Information")]

    [Export]
    public string Name {
        get => _name;
        set {
            if (ValidationService.ValidateName(value)) {
                _name = value;
                EmitChanged();
            }
        }
    }

    [Export(PropertyHint.MultilineText)]
    public string? Description { get; set; }

    [ExportGroup("🪵 Assets")]

    [Export]
    public Color Color { get; set; }

    [Export]
    public Texture2D? Icon {
        get => _icon;
        set {
            _icon = value;
            if (_icon is not null) {
                IconPath = _icon.ResourcePath;
                EmitChanged();
            }
        }
    }

    public string? IconPath { get; private set; }

    [ExportGroup("🔢 Values")]

    [Export]
    public int MinValue {
        get => _minValue;
        private set {
            if (ValidationService.ValidateMinMaxValues(value, MaxValue)) {
                _minValue = value;
                EmitChanged();
            }
        }
    }

    [Export]
    public int MaxValue {
        get => _maxValue;
        private set {
            if (ValidationService.ValidateMinMaxValues(MinValue, value)) {
                _maxValue = value;
                EmitChanged();
            }
        }
    }

    public AttributeType() {
    }

    public AttributeType(string name, string description, Color color, Texture2D icon, int minValue, int maxValue) {
        Name = name;
        Description = description;
        Color = color;
        Icon = icon;
        MinValue = minValue;
        MaxValue = maxValue;
        ValidateConstructor();
    }

    public void ValidateConstructor() {
        if (!ValidationService.ValidateName(Name)) {
            throw new ArgumentException("Invalid name", nameof(Name));
        }

        if (!ValidationService.ValidateMinMaxValues(MinValue, MaxValue)) {
            throw new ArgumentException("MinValue must be less than or equal to MaxValue");
        }
    }
}