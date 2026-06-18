namespace DiceRolling.Controllers;

/// Define os possíveis estados dos rounds.
public enum RoundState {
    RoundStart,
    ActionsDeclaration,
    TurnsResolution,
    RoundEnd,
}