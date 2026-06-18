using Godot;
using DiceRolling.Attributes;
using System.Linq;

namespace DiceRolling.Stores;

[Tool]
[GlobalClass]
public partial class AttributesStore : Resource {

    private static AttributesStore? _instance;
    public static AttributesStore Instance {
        get {
            if (_instance == null) {
                // TODO - achar um jeito melhor de acessar a coleção de atributos do jogo
                _instance = GD.Load<AttributesStore>("res://resources/Attributes/AttributesStore.tres");
                if (_instance == null) {
                    GD.PrintErr("Failed to load AttributesStore singleton from res://resources/Attributes/AttributesStore.tres. Creating new instance.");
                    _instance = new AttributesStore();
                }
            }
            return _instance;
        }
    }

    [Export]
    public Godot.Collections.Array<AttributeType> Attributes { get; private set; } = [];

    public AttributeType? GetAttributeByName(string name) {
        return Attributes.FirstOrDefault(attr => attr.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }

    public AttributeType? GetAttributeById(string id) {
        return Attributes.FirstOrDefault(attr => attr.Id.Equals(id, System.StringComparison.OrdinalIgnoreCase));
    }

    // Constructor
    public AttributesStore() {
        if (_instance == null) {
            _instance = this;
        }
    }
}