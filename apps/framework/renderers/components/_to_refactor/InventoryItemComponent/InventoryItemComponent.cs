using Godot;
using System;

namespace DiceRolling.Components.UI;

public partial class InventoryItemComponent : Control
{

    [Export]
    public PackedScene? TooltipComponentScene { get; set; }

    private TooltipComponent? _tooltipInstance;

    public override void _Ready()
    {
        Connect("mouse_entered", Callable.From(OnMouseEntered));
        Connect("mouse_exited", Callable.From(OnMouseExited));
        SetSize(new Vector2(64, 64));
    }

    private void SetSize(Vector2 size)
    {
        Size = size;
    }

    private void OnMouseEntered()
    {
        if (_tooltipInstance is null || !IsInstanceValid(_tooltipInstance))
        {
            CreateTooltip();
        }
        _tooltipInstance?.ShowTooltip(GetGlobalMousePosition());
    }

    private void OnMouseExited()
    {
        _tooltipInstance?.HideTooltip();
    }

    private void CreateTooltip()
    {
        if (TooltipComponentScene is null)
        {
            GD.PrintErr("TooltipComponentScene is null");
            throw new Exception("TooltipComponentScene is null");
        }

        _tooltipInstance = TooltipComponentScene.Instantiate<TooltipComponent>();
        _tooltipInstance.Initialize();
        _tooltipInstance.SetTooltipTexts("Title", "Tags", "Description", "Advanced Description", "Lore");
        // Add the tooltip to the root viewport
        GetTree().Root.AddChild(_tooltipInstance);
        _tooltipInstance.Visible = false;
    }
}
