using Godot;

namespace DiceRolling.Components.UI;

public partial class TooltipComponent : Control
{
	[Export]
	public NodePath? ProgressBarPath { get; set; }

	[Export(PropertyHint.Range, "0.1,10.0,0.1")]
	public float ProgressBarDuration { get; set; } = 1.0f;

	[Export]
	public NodePath? TextureIconPath { get; set; }

	[Export]
	public NodePath? TitlePath { get; set; }

	[Export]
	public NodePath? TagsPath { get; set; }

	[Export]
	public NodePath? DescriptionPath { get; set; }

	[Export]
	public NodePath? AdvancedDescriptionPath { get; set; }

	[Export]
	public NodePath? LorePath { get; set; }

	private ProgressBar? _progressBar;
	private Label? _title;
	private TextureRect? _textureIcon;
	private RichTextLabel? _tags;
	private RichTextLabel? _description;
	private RichTextLabel? _advancedDescription;
	private RichTextLabel? _lore;
	private Timer? _tooltipTimer;
	private bool _isMouseHovering = false;

	public override void _Ready()
	{
		Initialize();
	}

	public void Initialize()
	{
		_progressBar = GetNodeOrNull<ProgressBar>(ProgressBarPath);
		_title = GetNodeOrNull<Label>(TitlePath);
		_textureIcon = GetNodeOrNull<TextureRect>(TextureIconPath);
		_tags = GetNodeOrNull<RichTextLabel>(TagsPath);
		_description = GetNodeOrNull<RichTextLabel>(DescriptionPath);
		_advancedDescription = GetNodeOrNull<RichTextLabel>(AdvancedDescriptionPath);
		_lore = GetNodeOrNull<RichTextLabel>(LorePath);

		_tooltipTimer = new Timer();
		_tooltipTimer.WaitTime = 0.05f; // Update every 0.05 seconds
		_tooltipTimer.Connect("timeout", Callable.From(OnTooltipTimerTimeout));
		AddChild(_tooltipTimer);
	}

	public void SetTooltipTexts(string title, string tags, string description, string advancedDescription, string lore)
	{
		if (_title is null || _tags is null || _description is null || _advancedDescription is null || _lore is null)
		{
			GD.PrintErr("One or more tooltip nodes are null");
			return;
		}

		_title.Text = title;
		_tags.Text = tags;
		_description.Text = description;
		_advancedDescription.Text = advancedDescription;
		_lore.Text = lore;
	}

	public void ShowTooltip(Vector2 mousePosition)
	{
		_isMouseHovering = true;
		Visible = true;
		MouseFilter = MouseFilterEnum.Ignore;
		if (_progressBar is not null)
		{
			_progressBar.Value = 0;
		}
		_tooltipTimer?.Start();
		PositionTooltip(mousePosition);
	}

	public void HideTooltip()
	{
		_isMouseHovering = false;
		MouseFilter = MouseFilterEnum.Pass;
		if (_progressBar is not null && _progressBar.Value < 100)
		{
			QueueFree();
		}
	}

	private void OnTooltipTimerTimeout()
	{
		if (_isMouseHovering)
		{
			if (_progressBar is not null)
			{
				_progressBar.Value += 100.0f / (ProgressBarDuration / _tooltipTimer!.WaitTime);
				if (_progressBar.Value >= 100)
				{
					_tooltipTimer.Stop();
				}
			}
		}
		else
		{
			if (_progressBar is not null && _progressBar.Value < 100)
			{
				QueueFree();
			}
		}
	}

	private void PositionTooltip(Vector2 mousePosition)
	{
		Vector2 screenSize = GetViewportRect().Size;
		Vector2 tooltipSize = Size;

		Vector2 position = mousePosition + new Vector2(10, 10); // Offset from the mouse cursor

		// Ensure the tooltip doesn't overflow the screen
		if (position.X + tooltipSize.X > screenSize.X)
		{
			position.X = screenSize.X - tooltipSize.X;
		}
		if (position.Y + tooltipSize.Y > screenSize.Y)
		{
			position.Y = screenSize.Y - tooltipSize.Y;
		}

		GlobalPosition = position;
	}
}
