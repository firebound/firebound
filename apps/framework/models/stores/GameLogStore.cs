using Godot;
using System.Collections.Generic;

using DiceRolling.Logs;

namespace DiceRolling.Stores;

[Tool]
[GlobalClass]
public partial class GameLogStore : Node {
    private static GameLogStore? _instance;
    public static GameLogStore Instance {
        get {
            _instance ??= new GameLogStore();
            return _instance;
        }
    }

    [Signal]
    public delegate void GameLogUpdatedEventHandler();

    [Signal]
    public delegate void GameLogLineAddedEventHandler();

    public List<GameLogMessage> Messages { get; private set; } = new List<GameLogMessage>();

    private GameLogStore() {
        AddUserSignal(nameof(GameLogUpdatedEventHandler));
        AddUserSignal(nameof(GameLogLineAddedEventHandler));
    }

    public void AddGameLogMessage(GameLogMessage message) {
        // GD.Print("AddGameLogMessage called with heading: ", message.Heading);
        Messages.Add(message);
        EmitSignal(nameof(GameLogUpdatedEventHandler));
    }

    public void AddGameLogLine(GameLogLine line) {
        // GD.Print("AddGameLogLine called with text: ", line.Text);
        if (Messages.Count == 0) {
            GD.PrintErr("No messages in GameLogStore to add a line to.");
            return;
        }
        Messages[^1].Lines.Add(line);
        EmitSignal(nameof(GameLogLineAddedEventHandler));
    }
}