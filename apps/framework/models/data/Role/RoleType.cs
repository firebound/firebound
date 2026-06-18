using Godot;
using System;
using DiceRolling.Id;
using DiceRolling.Services;

namespace DiceRolling.Roles;

[Tool]
[GlobalClass]
public partial class RoleType : IdentifiableResource, IRole {
    private string _name = "Role_" + Guid.NewGuid().ToString("N");

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
    public Godot.Collections.Array<RoleAttribute> RoleAttributes { get; set; } = [];

    [Export]
    public Godot.Collections.Array<RoleAction> RoleActions { get; set; } = [];

    public RoleType() {
        ValidateConstructor();
    }

    public RoleType(string name, string? description, Godot.Collections.Array<RoleAttribute> roleAttributes, Godot.Collections.Array<RoleAction> roleActions) {
        Name = name;
        Description = description;
        RoleAttributes = roleAttributes;
        RoleActions = roleActions;
        ValidateConstructor();
    }

    public void ValidateConstructor() {
        if (!ValidationService.ValidateName(Name)) {
            throw new ArgumentException("Invalid name", nameof(Name));
        }
    }
}