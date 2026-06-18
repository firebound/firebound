using System;
using System.Collections.Generic;

namespace DiceRolling.Logs;

public class GameLogMessage {
    public string Heading { get; }
    public string Timestamp { get; }
    public List<GameLogLine> Lines { get; }

    public GameLogMessage(string heading, string timestamp, List<GameLogLine> lines) {
        if (string.IsNullOrWhiteSpace(heading)) {
            throw new ArgumentException("Heading cannot be null or empty", nameof(heading));
        }
        if (string.IsNullOrWhiteSpace(timestamp)) {
            throw new ArgumentException("Timestamp cannot be null or empty", nameof(timestamp));
        }
        Heading = heading;
        Timestamp = timestamp;
        Lines = lines ?? throw new ArgumentNullException(nameof(lines));
    }
}
