using Godot;
using DiceRolling.Id;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
[Icon("res://assets/editor/entity-3d.svg")]
public abstract partial class Entity3D : Node3D {
    [Signal] public delegate void EntityUpdatedEventHandler();

    private IdentifiableResource? _data;

    public IdentifiableResource? Data {
        get => _data;
        set {
            if (_data == value) {
                return;
            }
            _data = value;
            NotifyUpdate();
        }
    }

    protected void NotifyUpdate() {
        EmitSignal(nameof(EntityUpdated));
        if (Engine.IsEditorHint()) {
            NotifyPropertyListChanged();
        }
    }

    public T? GetData<T>() where T : IdentifiableResource {
        return Data as T;
    }
}