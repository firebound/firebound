using Godot;
using System;

using DiceRolling.Dice;
using DiceRolling.Targets;
using DiceRolling.Effects;
using DiceRolling.Services;
using DiceRolling.Id;
using DiceRolling.Categories;

namespace DiceRolling.Actions;

/// <summary>
/// Representa um tipo de ação no jogo, incluindo suas informações, categoria, descrição, ícone, energia necessária, efeitos e configuração de alvo.
/// Esta classe também fornece métodos para inicializar e gerenciar esses aspectos.
/// </summary>
[Tool]
[GlobalClass]
public partial class ActionType : IdentifiableResource, IAction<IActionContext, bool> {
    private string _name = "Action_" + Guid.NewGuid().ToString("N");
    private Texture2D? _icon;

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

    [Export]
    public Category? Category { get; set; }

    [Export(PropertyHint.MultilineText)]
    public string? Description { get; set; }

    [ExportGroup("🪵 Assets")]

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

    [ExportGroup("🎭 Behavior")]

    [Export]
    public Godot.Collections.Array<DiceEnergy> RequiredEnergy { get; set; } = [];

    [Export]
    public Godot.Collections.Array<EffectType> Effects { get; set; } = [];

    [Export]
    public TargetBoardType? TargetBoard { get; set; }

    public ActionType() {
    }

    public ActionType(
        string name,
        Category category,
        string? description,
        Texture2D? icon,
        Godot.Collections.Array<DiceEnergy> requiredEnergy,
        Godot.Collections.Array<EffectType> effects,
        TargetBoardType? targetBoard
    ) {
        Name = name;
        Category = category;
        Description = description;
        Icon = icon;
        RequiredEnergy = requiredEnergy;
        Effects = effects;
        TargetBoard = targetBoard;
        ValidateConstructor();
    }

    public bool Do(IActionContext context) {
        EffectService.ApplyEffects(Effects, context);
        return true;
    }

    // Métodos de utilidade
    public bool IsValid() {
        return !string.IsNullOrEmpty(Id) &&
               Category != null &&
               RequiredEnergy.Count > 0 &&
               Effects.Count > 0 &&
               TargetBoard != null;
    }

    public void AddEffect(EffectType effect) {
        if (!Effects.Contains(effect)) {
            Effects.Add(effect);
        }
    }

    public void RemoveEffect(EffectType effect) {
        Effects.Remove(effect);
    }

    public void ValidateConstructor() {
        if (!ValidationService.ValidateName(Name)) {
            throw new ArgumentException("Invalid name", nameof(Name));
        }
    }
}