using Godot;

namespace DiceRolling.Controllers;

public enum MenuScenes {
    MainMenu,
    GameplayLobby,
    GameOverMenu
}

[GlobalClass]
public partial class MenuTransitionManager : Node {
    [Export]
    public PackedScene? MainMenuScene { get; set; }

    [Export]
    public PackedScene? GameplayLobbyScene { get; set; }

    [Export]
    public PackedScene? GameOverMenuScene { get; set; }

    public void TransitionTo(MenuScenes scene) {
        switch (scene) {
            case MenuScenes.MainMenu:
                GetTree().ChangeSceneToPacked(MainMenuScene);
                break;
            case MenuScenes.GameplayLobby:
                GetTree().ChangeSceneToPacked(GameplayLobbyScene);
                break;
            case MenuScenes.GameOverMenu:
                GetTree().ChangeSceneToPacked(GameOverMenuScene);
                break;
        }
    }
}