using System;

namespace DiceRolling.Logs;

public enum GameLogLineType {
    Default,
    Error,
    Info,
    Success,
    Warning,
    Wip,
    Action,
    Combat
}

public class GameLogLine {
    public GameLogLineType Type { get; }
    public string Text { get; }

    public GameLogLine(GameLogLineType type, string text) {
        Type = type;
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }
}
