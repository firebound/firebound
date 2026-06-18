using Godot;

namespace DiceRolling.Controllers;

public enum GameplayScenes {
    GameplayLobby,
    GameplayDungeon,
    GameplayBattle
}

public partial class GameplayTransitionManager : Node {
    [Export]
    public PackedScene? GameplayLobbyScene { get; set; }

    [Export]
    public PackedScene? GameplayDungeonScene { get; set; }

    [Export]
    public PackedScene? GameplayBattleScene { get; set; }

    public void TransitionTo(GameplayScenes scene) {
        switch (scene) {
            case GameplayScenes.GameplayLobby:
                GetTree().ChangeSceneToPacked(GameplayLobbyScene);
                break;
            case GameplayScenes.GameplayDungeon:
                GetTree().ChangeSceneToPacked(GameplayDungeonScene);
                break;
            case GameplayScenes.GameplayBattle:
                GetTree().ChangeSceneToPacked(GameplayBattleScene);
                break;
        }
    }
}