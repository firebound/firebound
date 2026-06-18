using Godot;

namespace DiceRolling.Id;

public interface IIdentifiable {
    string Id { get; }
    Callable GenerateNewIdButton { get; }
}