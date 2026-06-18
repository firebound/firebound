using Godot;
using DiceRolling.Id;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
[Icon("res://assets/editor/entity-ui.svg")]
public abstract partial class EntityControl : Control {
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
        EmitSignal(SignalName.EntityUpdated);
    }

    public T? GetData<T>() where T : IdentifiableResource {
        return Data as T;
    }
}