using Godot;
using DiceRolling.Grids;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
public partial class GridCellEntity : Entity3D {

    [ExportGroup("Data")]
    public GridCellType? CellData => GetData<GridCellType>();

    [ExportGroup("Preview")]
    [Export] public GridCellType? PreviewCellData { get; set; }

    [ExportToolButton("Update Preview")]
    public Callable UpdatePreviewData => Callable.From(() => {
        if (Engine.IsEditorHint()) {
            if (Data != PreviewCellData) {
                Data = PreviewCellData;
            }
            else {
                NotifyUpdate();
            }
        }
    });

    public override void _Ready() {
        base._Ready();

        if (Engine.IsEditorHint()) {
            if (PreviewCellData != null) {
                if (Data != PreviewCellData) {
                    Data = PreviewCellData;
                }
            }
        }
        else {
            // Runtime specific logic for GridCellEntity (if any)
        }
    }
}