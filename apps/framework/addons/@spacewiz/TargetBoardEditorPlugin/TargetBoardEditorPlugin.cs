using Godot;

namespace DiceRolling.Editor;

[Tool]
public partial class TargetBoardEditorPlugin : EditorPlugin {
    public override void _EnterTree() {
        AddInspectorPlugin(new TargetBoardInspectorPlugin());
    }

    public override void _ExitTree() {
        RemoveInspectorPlugin(new TargetBoardInspectorPlugin());
    }
}
