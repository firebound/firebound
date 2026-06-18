using DiceRolling.Controllers;
using Godot;

namespace DiceRolling.UI;

public enum TransitionType {
    None,
    Menu,
    Gameplay
}

[Tool]
public partial class SceneTransitionButton : Button {
    private TransitionType _typeOfTransition = TransitionType.None;
    [Export]
    public TransitionType TypeOfTransition {
        get => _typeOfTransition;
        set {
            if (_typeOfTransition != value) {
                _typeOfTransition = value;
                NotifyPropertyListChanged();
            }
        }
    }

    [Export]
    public MenuScenes MenuScene { get; set; } = MenuScenes.MainMenu;

    [Export]
    public GameplayScenes GameplayScene { get; set; } = GameplayScenes.GameplayLobby;

    private MenuTransitionManager? _menuTransitionManager;
    private GameplayTransitionManager? _gameplayTransitionManager;

    public override void _Ready() {
        if (!Engine.IsEditorHint()) {
            _menuTransitionManager = GetNode<MenuTransitionManager>("/root/MenuTransitionManager");
            _gameplayTransitionManager = GetNode<GameplayTransitionManager>("/root/GameplayTransitionManager");

            this.Pressed += OnButtonPressed;
        }
    }

    private void OnButtonPressed() {
        switch (TypeOfTransition) {
            case TransitionType.Menu:
                _menuTransitionManager?.TransitionTo(MenuScene);
                break;
            case TransitionType.Gameplay:
                _gameplayTransitionManager?.TransitionTo(GameplayScene);
                break;
        }
    }

    public override void _ValidateProperty(Godot.Collections.Dictionary property) {
        if (property["name"].AsStringName() == "MenuScene" && TypeOfTransition != TransitionType.Menu) {
            var usage = property["usage"].As<PropertyUsageFlags>() | PropertyUsageFlags.ReadOnly;
            property["usage"] = (int)usage;
        }
        if (property["name"].AsStringName() == "GameplayScene" && TypeOfTransition != TransitionType.Gameplay) {
            var usage = property["usage"].As<PropertyUsageFlags>() | PropertyUsageFlags.ReadOnly;
            property["usage"] = (int)usage;
        }
    }
}
