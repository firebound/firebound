using Godot;
using DiceRolling.Targets;
using DiceRolling.Grids;

namespace DiceRolling.Editor;

[Tool]
public partial class TargetBoardInspectorPlugin : EditorInspectorPlugin {
    private BoardMatrixEditor? boardMatrixEditor;
    private CheckBox? flipCheckBox;
    private Button? clearGridButton;

    public override bool _CanHandle(GodotObject @object) {
        return @object is TargetBoardType;
    }

    public override void _ParseBegin(GodotObject @object) {
        if (@object is TargetBoardType targetBoard) {
            var container = new VBoxContainer();
            AddCustomControl(container);

            // Initialize and add BoardMatrixEditor
            boardMatrixEditor = new BoardMatrixEditor(targetBoard);
            container.AddChild(boardMatrixEditor);

            // Clear existing grids before adding new ones
            boardMatrixEditor.ClearGrids();

            // Add grids to BoardMatrixEditor
            foreach (var grid in targetBoard.Grids) {
                if (IsValidGridConfiguration(grid)) {
                    boardMatrixEditor.AddGrid(grid);
                }
                else {
                    GD.PrintErr($"Invalid grid configuration: Rows = {grid.Rows}, Columns = {grid.Columns}");
                }
            }

            // Add Flip CheckBox
            flipCheckBox = new CheckBox { Text = "Flip Horizontally (preview)" };
            flipCheckBox.Toggled += OnFlipCheckBoxToggled;
            container.AddChild(flipCheckBox);

            // Add Clear Grid Button
            clearGridButton = new Button { Text = "Clear Grid Inputs" };
            clearGridButton.Pressed += OnClearGridButtonPressed;
            container.AddChild(clearGridButton);

            // Add description RichTextLabel
            var descriptionRichTextLabel = new RichTextLabel {
                BbcodeEnabled = true,
                Text = "[b]Possible values on the matrix:[/b]\n" +
                       "[color=white]0 - Ignored[/color]\n" +
                       "[color=yellow]1 - Actor Placement[/color]\n" +
                       "[color=green]2 - Ally[/color]\n" +
                       "[color=red]3 - Target[/color]\n",
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                SizeFlagsVertical = Control.SizeFlags.ExpandFill,
                CustomMinimumSize = new Vector2(0, 80)
            };
            container.AddChild(descriptionRichTextLabel);

            // Connect SetupChanged signal
            if (!targetBoard.IsConnected(nameof(TargetBoardType.SetupChanged), new Callable(this, nameof(OnSetupChanged)))) {
                targetBoard.Connect(nameof(TargetBoardType.SetupChanged), new Callable(this, nameof(OnSetupChanged)));
            }
        }
    }

    private static bool IsValidGridConfiguration(GridType grid) {
        return grid.Rows > 0 && grid.Columns > 0;
    }

    private static void OnSetupChanged() {
        GD.Print("Setup changed.");
    }

    private void OnFlipCheckBoxToggled(bool toggled) {
        boardMatrixEditor?.FlipHorizontally(toggled);
    }

    private void OnClearGridButtonPressed() {
        boardMatrixEditor?.ClearGridInputs();
    }
}