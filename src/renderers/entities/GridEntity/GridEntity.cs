using Godot;
using DiceRolling.Grids;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
public partial class GridEntity : Entity3D {

    public GridType? GridData => GetData<GridType>();

    [ExportGroup("Preview")]
    [Export] public GridType? PreviewGridData { get; set; }

    [ExportToolButton("Update Preview")]
    public Callable UpdatePreviewData => Callable.From(() => {
        if (Engine.IsEditorHint()) {
            Data = PreviewGridData;
            NotifyUpdate();
        }
    });

    public override void _Ready() {
        base._Ready();

        if (Engine.IsEditorHint()) {
            if (PreviewGridData != null) {
                if (Data != PreviewGridData) {
                    Data = PreviewGridData;
                }
            }
        }
        else {
            // Runtime specific logic for GridEntity (if any)
        }
    }
}