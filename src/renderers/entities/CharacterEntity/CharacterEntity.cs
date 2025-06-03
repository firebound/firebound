using Godot;
using DiceRolling.Characters;

namespace DiceRolling.Entities;

[Tool]
[GlobalClass]
public partial class CharacterEntity : Entity3D {

    public CharacterType? CharacterData => GetData<CharacterType>();

    [ExportGroup("Preview")]
    [Export] public CharacterType? PreviewCharacterData { get; set; }

    [ExportToolButton("Update Preview")]
    public Callable UpdatePreviewData => Callable.From(() => {
        if (Engine.IsEditorHint()) {
            Data = PreviewCharacterData;
            NotifyUpdate();
        }
    });

    public override void _Ready() {
        base._Ready();

        if (Engine.IsEditorHint()) {
            if (PreviewCharacterData != null) {
                if (Data != PreviewCharacterData) {
                    Data = PreviewCharacterData;
                }
            }
        }
        else {
            // Runtime specific logic for CharacterEntity (if any)
        }
    }
}