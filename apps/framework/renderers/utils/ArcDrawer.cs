using Godot;
using DiceRolling.Helpers;

namespace DiceRolling.UI;

public partial class ArcDrawer : Node2D {
    // private CharacterComponent? _selectedCharacter;
    // private CharacterComponent? _selectedEnemy;
    private Arc2D? _arc;
    private bool _isDrawing;

    public override void _Ready() {
        if (Engine.IsEditorHint()) return;

        _arc = new Arc2D();
        AddChild(_arc);
    }

    // public override void _Process(double delta) {
    //     if (_isDrawing && _selectedCharacter is not null) {
    //         Vector2 mousePosition = GetGlobalMousePosition();
    //         DrawArc(_selectedCharacter.GlobalPosition, mousePosition);
    //     }
    // }

    // public void SetSelectedCharacter(CharacterComponent character)
    // {
    //     _selectedCharacter = character;
    //     _selectedEnemy = null; // Reset the selected enemy
    //     _isDrawing = true;
    //     QueueRedraw();
    // }

    // public void SetSelectedEnemy(CharacterComponent enemy) {
    //     _selectedEnemy = enemy;
    //     _isDrawing = false;
    //     if (_selectedCharacter is not null) {
    //         DrawArc(_selectedCharacter.GlobalPosition, _selectedEnemy.GlobalPosition);
    //     }
    //     QueueRedraw();
    // }

    private void DrawArc(Vector2 start, Vector2 end) {
        if (_arc is not null) {
            _arc.Points = new Vector2[] { start, end };
            _arc.QueueRedraw();
        }
        else {
            GD.PrintErr("_arc is null, cannot draw arc.");
        }
    }
}