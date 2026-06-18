using System;
using Godot;

using DiceRolling.Id;
using DiceRolling.Services;

namespace DiceRolling.Dice;

[Tool]
[GlobalClass]
public partial class DiceEnergy : IdentifiableResource, IDiceEnergy {
    private string _name = "Energy_" + Guid.NewGuid().ToString("N");
    private Texture2D? _icon;

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

    [Export]
    public Color BackgroundColor { get; set; }

    [Export]
    public Color MainColor { get; set; }

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

    public DiceEnergy() {
    }

    public DiceEnergy(string name, string description, Color backgroundColor, Color mainColor, Texture2D icon) {
        Name = name;
        Description = description;
        BackgroundColor = backgroundColor;
        MainColor = mainColor;
        Icon = icon;
    }
}