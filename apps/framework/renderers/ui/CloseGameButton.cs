using Godot;

namespace DiceRolling.UI
{
    [Tool]
    public partial class CloseGameButton : Button
    {
        public override void _Ready()
        {
            Pressed += OnButtonPressed;
        }

        private void OnButtonPressed()
        {
            GetTree().Quit();
        }
    }
}