using Godot;

namespace DiceRolling.Helpers;

/// <summary>
/// Provides extension methods for Godot Node operations
/// </summary>
public static class NodeExtensions {
    /// <summary>
    /// Recursively searches downwards for a node of the specified type starting from the given root node.
    /// </summary>
    /// <typeparam name="T">The type of node to find</typeparam>
    /// <param name="root">The root node to start searching from</param>
    /// <returns>The first node of type T found, or null if none exists</returns>
    public static T? FindNodeOfType<T>(this Node root) where T : class {
        if (root is T nodeOfType) {
            // GD.PrintRich($"[color=green]Found node of type {typeof(T)}: {nodeOfType}[/color]");
            return nodeOfType;
        }

        foreach (Node child in root.GetChildren()) {
            // GD.Print($"Searching in child node: {child}");
            var result = FindNodeOfType<T>(child);
            if (result != null) {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Searches upwards in the scene tree for the first ancestor of the specified type T.
    /// </summary>
    /// <typeparam name="T">The type of ancestor node to find.</typeparam>
    /// <param name="node">The node to start searching upwards from.</param>
    /// <returns>The first ancestor node of type T found, or null if none exists.</returns>
    public static T? FindAncestorOfType<T>(this Node node) where T : Node {
        Node? current = node.GetParent();
        while (current != null) {
            if (current is T ancestor) {
                return ancestor;
            }
            current = current.GetParent();
        }
        return null;
    }
}