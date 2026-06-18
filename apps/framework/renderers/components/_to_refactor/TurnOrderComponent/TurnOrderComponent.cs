using Godot;
using System.Linq;
using System.Collections.Generic;
using DiceRolling.Helpers;
using DiceRolling.Events;
using DiceRolling.Characters;
using DiceRolling.Attributes;
using DiceRolling.Stores;

namespace DiceRolling.Components;

[Tool]
public partial class TurnOrderComponent : Control {
    private AttributeType? SpeedAttributeType;
    private AttributeType? HealthAttributeType;

    private AttributesStore? _AttributesStore;
    [ExportGroup("🪵 Resources")]
    [Export] private Resource? AttributeConfigResource;

    private CharacterType[] _characters = [];
    [Export]
    public CharacterType[] Characters {
        get => _characters;
        set {
            _characters = value;
            if (SpeedAttributeType is null || HealthAttributeType is null) {
                return;
            }
            if (_characters.Length > 0) {
                // Filter out null or uninitialized characters
                var validCharacters = _characters.Where(c => c is not null).ToList();
                if (validCharacters.Count > 0) {
                    UpdateTurnOrder(validCharacters);
                }
                else {
                    GD.PrintErr("No valid characters found.");
                }
            }
            else {
                GD.PrintErr("Characters array is empty.");
            }
        }
    }

    [ExportGroup("🔘 Nodes")]
    [Export] public HBoxContainer? PortraitsContainerNode { get; set; }

    private PanelContainer? _portraitTemplate;

    [ExportSubgroup("🖼️ Portrait Template")]
    [Export]
    PanelContainer? PortraitTemplateNode {
        get => _portraitTemplate;
        set {
            if (value is not null) {
                _portraitTemplate = value;
            }
        }
    }

    [Export] public Panel? PortraitPanelNode;
    public string PortraitPanelName => PortraitPanelNode?.Name ?? "PortraitPanel";

    [Export] public TextureRect? PortraitTextureNode;
    public string PortraitTextureName => PortraitTextureNode?.Name ?? "PortraitTexture";

    [Export] public ColorRect? PortraitDamageColorNode;
    public string PortraitDamageColorName => PortraitDamageColorNode?.Name ?? "PortraitDamageColor";

    public override void _Ready() {
        if (AttributeConfigResource is AttributesStore attributeConfig) {
            _AttributesStore = attributeConfig;
            // SpeedAttributeType = AttributesHelper.GetAttributeType(_AttributesStore, "Speed");
            // HealthAttributeType = AttributesHelper.GetAttributeType(_AttributesStore, "Health");

            // Update turn order if characters are already set
            if (_characters.Length > 0) {
                var validCharacters = _characters.Where(c => c is not null).ToList();
                if (validCharacters.Count > 0) {
                    UpdateTurnOrder(validCharacters);
                }
            }
        }

        // Connect the AttributeChanged signal from EventBus to the OnCharacterAttributeChanged method
        // EventBus.Instance.Connect(nameof(EventBus.AttributeChanged), new Callable(this, nameof(OnCharacterAttributeChanged)));

    }

    private void OnCharacterAttributeChanged(AttributeType attributeType) {
        if (attributeType == HealthAttributeType) {
            UpdateTurnOrder([.. Characters]);
        }
    }

    private void SetupPortraitInstance(CharacterType character, PanelContainer portraitInstance) {
        if (PortraitPanelNode is null || PortraitTextureNode is null || PortraitDamageColorNode is null) {
            GD.PrintErr("One or more portrait nodes are not set.");
            return;
        }

        if (PortraitPanelNode is null || PortraitTextureNode is null || PortraitDamageColorNode is null) {
            GD.PrintErr("One or more portrait nodes are not set.");
            return;
        }

        // Get the Panel node from the portrait instance
        var panel = portraitInstance.GetNodeOrNull<Panel>(PortraitPanelName);
        if (panel is null) {
            GD.PrintErr("Panel node not found in portrait instance.");
            return;
        }

        // Get the TextureRect and ColorRect nodes from the Panel node
        var textureRect = panel.GetNodeOrNull<TextureRect>(PortraitTextureName);
        var damageColor = panel.GetNodeOrNull<ColorRect>(PortraitDamageColorName);

        portraitInstance.Visible = true;

        if (textureRect is not null && character.Portrait is not null) {
            textureRect.Texture = character.Portrait;
        }
        else {
            GD.PrintErr("TextureRect or character portrait is null for character: ", character.Name);
        }

        int currentHealth = HealthAttributeType is not null ? character.GetAttributeCurrentValue(HealthAttributeType) : 0;
        int maxHealth = HealthAttributeType is not null ? character.GetAttributeMaxValue(HealthAttributeType) : 0;

        if (damageColor is not null && maxHealth > 0) {
            float damageRatio = (float)(maxHealth - currentHealth) / maxHealth;
            damageColor.Scale = new Vector2(1, damageRatio);
        }
    }
    public void UpdateTurnOrder(List<CharacterType> characters) {
        if (PortraitsContainerNode is null || PortraitTemplateNode is null) {
            return;
        }
        if (SpeedAttributeType is null || HealthAttributeType is null) {
            return;
        }

        foreach (Node child in PortraitsContainerNode.GetChildren()) {
            if (child != PortraitTemplateNode) {
                PortraitsContainerNode.RemoveChild(child);
                child.QueueFree();
            }
        }

        var sortedCharacters = characters;
        //     .Where(c => c is not null && c.GetAttributeCurrentValue(SpeedAttributeType) != 0)
        //     .OrderByDescending(c => c.GetAttributeCurrentValue(SpeedAttributeType))
        //     .ToList();


        foreach (var character in sortedCharacters) {
            if (character is null) {
                GD.PrintErr("Character is null, skipping...");
                continue;
            }

            if (PortraitTemplateNode.Duplicate() is not PanelContainer portraitInstance) {
                GD.PrintErr("Failed to duplicate PortraitTemplateNode for character: ", character.Name);
                continue;
            }

            SetupPortraitInstance(character, portraitInstance);
            PortraitsContainerNode.AddChild(portraitInstance);
        }
    }
}