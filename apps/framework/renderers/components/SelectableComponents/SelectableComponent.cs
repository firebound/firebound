using Godot;
using DiceRolling.Entities;
using DiceRolling.Events;

namespace DiceRolling.Components;

/// <summary>
/// Component that allows for selection of an entity.
/// </summary>
/// <remarks>
/// This component listens for input events and allows for selection of the parent entity.
/// </remarks>
[Tool]
[GlobalClass]
[Icon("res://assets/editor/component-3d.svg")]
public partial class SelectableComponent : Node3D {
    private Entity3D? _parent;

    private static SelectableComponent? _currentlySelectedComponent;

    private bool _isSelected = false;

    private bool _isHovered = false;

    [Export] public Sprite3D? SelectorSprite { get; set; }
    [Export] public Sprite3D? HoverSpriteNode { get; set; }
    [Export] public Area3D? InputAreaNode { get; set; }

    [Export]
    public bool IsSelected {
        get => _isSelected;
        set {
            if (_isSelected != value) {
                _isSelected = value;
                OnIsSelectedSet(value);
            }
        }
    }

    [Export]
    public bool IsHovered {
        get => _isHovered;
        set {
            if (_isHovered != value) {
                _isHovered = value;
                OnIsHoveredSet(value);
            }
        }
    }

    public override void _Ready() {
        _parent = GetParent<Entity3D>();

        // Initialize visuals
        if (SelectorSprite is not null) {
            SelectorSprite.Visible = false;
        }
        if (HoverSpriteNode is not null) {
            HoverSpriteNode.Visible = false;
        }

        ConnectSignals();

    }

    public override void _ExitTree() {
        DisconnectSignals();
    }


    private void ConnectSignals() {
        if (_parent != null) {
            _parent.EntityUpdated += OnEntityUpdated;
        }

        EventBus.Instance.ComponentUnselected += OnComponentUnselected;

        if (InputAreaNode != null) {
            InputAreaNode.InputEvent += OnInputEvent;
            InputAreaNode.MouseEntered += OnMouseEntered;
            InputAreaNode.MouseExited += OnMouseExited;
        }
    }

    private void DisconnectSignals() {
        if (_parent != null) {
            _parent.EntityUpdated -= OnEntityUpdated;
        }

        EventBus.Instance.ComponentUnselected -= OnComponentUnselected;

        if (InputAreaNode != null) {
            InputAreaNode.InputEvent -= OnInputEvent;
            InputAreaNode.MouseEntered -= OnMouseEntered;
            InputAreaNode.MouseExited -= OnMouseExited;
        }
    }

    private void OnComponentUnselected(Node component) {
        if (component == this) {
            IsSelected = false;
        }
    }

    private void OnMouseEntered() {
        IsHovered = true;
    }

    private void OnMouseExited() {
        IsHovered = false;
    }

    private static void OnEntityUpdated() {
    }

    private void OnInputEvent(Node camera, InputEvent @event, Vector3 clickPosition, Vector3 normal, long shapeIdx) {
        if (@event is InputEventMouseButton { Pressed: true }) {
            HandleSelection(this);
        }
    }

    private static void HandleSelection(SelectableComponent current) {
        if (_currentlySelectedComponent == current) {
            current.IsSelected = false;
            _currentlySelectedComponent = null;
            EventBus.Instance.OnComponentUnselected(current);
        }
        else {
            if (_currentlySelectedComponent is not null) {
                _currentlySelectedComponent.IsSelected = false;
                EventBus.Instance.OnComponentUnselected(_currentlySelectedComponent);
            }
            current.IsSelected = true;
            _currentlySelectedComponent = current;
            EventBus.Instance.OnComponentSelected(current);
        }
    }

    private void OnIsSelectedSet(bool isSelected) {
        if (SelectorSprite is not null) {
            SelectorSprite.Visible = isSelected;
        }
    }

    private void OnIsHoveredSet(bool isHovered) {
        if (HoverSpriteNode is not null) {
            HoverSpriteNode.Visible = isHovered;
        }
    }
}