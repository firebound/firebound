using Godot;

namespace DiceRolling.Editor;

/// <summary>
/// A plugin for managing the Arc3D gizmo in the editor.
/// </summary>
[Tool]
public partial class Arc3DEditorPlugin : EditorPlugin
{
    private Arc3DGizmoPlugin? _gizmoPlugin;

    /// <summary>
    /// Called when the plugin is added to the editor.
    /// <inheritdoc cref="Godot.Node._EnterTree"/>
    /// </summary>
    public override void _EnterTree()
    {
        _gizmoPlugin = new Arc3DGizmoPlugin();
        AddNode3DGizmoPlugin(_gizmoPlugin);
    }

    /// <summary>
    /// Called when the plugin is removed from the editor.
    /// <inheritdoc cref="Godot.Node._ExitTree"/>
    /// </summary>
    public override void _ExitTree()
    {
        RemoveNode3DGizmoPlugin(_gizmoPlugin);
    }
}