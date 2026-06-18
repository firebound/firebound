using Godot;
using DiceRolling.Logs;
using DiceRolling.Stores;

namespace DiceRolling.Components.UI;

public partial class GameLogComponent : ScrollContainer {
    [Export] public BoxContainer? _messageContainerNode;
    [Export] public VBoxContainer? _messageTemplateNode;
    [Export] public Label? _headingLabelTemplateNode;
    [Export] public Label? _timestampLabelTemplateNode;
    [Export] public RichTextLabel? _lineTemplateNode;

    public override void _Ready() {
        // Connect to the GameLogStore signals
        GameLogStore.Instance.Connect("GameLogUpdatedEventHandler", Callable.From(OnGameLogUpdated));
        GameLogStore.Instance.Connect("GameLogLineAddedEventHandler", Callable.From(OnGameLogLineAdded));
        // Initialize the game log
        UpdateGameLog();
    }

    private void OnGameLogUpdated() {
        AddMessageToLog(GameLogStore.Instance.Messages[^1]);
    }

    private void OnGameLogLineAdded() {
        UpdateLastMessageLines();
    }

    private void UpdateGameLog() {
        if (_messageTemplateNode is null) {
            GD.PrintErr("MessageTemplate node is not assigned.");
            return;
        }

        _messageTemplateNode.Visible = false;

        foreach (var message in GameLogStore.Instance.Messages) {
            AddMessageToLog(message);
        }
    }

    private void AddMessageToLog(GameLogMessage message) {
        if (_messageTemplateNode is null || _messageContainerNode is null) {
            GD.PrintErr("MessageTemplate or MessageContainer node is not assigned.");
            return;
        }

        // Duplicate the message template
        var messageTemplate = (VBoxContainer)_messageTemplateNode.Duplicate();
        messageTemplate.Visible = true;

        // Clear template texts before updating
        ClearTemplateTexts(messageTemplate);

        // Update the header and timestamp
        UpdateHeaderAndTimestamp(messageTemplate, message);

        // Update the lines
        UpdateLines(messageTemplate, message);

        // Add the message template to the container
        _messageContainerNode.AddChild(messageTemplate);
    }

    private void UpdateLastMessageLines() {
        if (_messageContainerNode is null) {
            GD.PrintErr("MessageContainer node is not assigned.");
            return;
        }

        var lastMessageTemplate = _messageContainerNode.GetChild<VBoxContainer>(_messageContainerNode.GetChildCount() - 1);
        var lastMessage = GameLogStore.Instance.Messages[^1];

        // Update the lines
        UpdateLines(lastMessageTemplate, lastMessage);
    }

    private static void UpdateHeaderAndTimestamp(VBoxContainer messageTemplate, GameLogMessage message) {
        var headerTemplate = messageTemplate.GetNode<HBoxContainer>("HeaderTemplate");
        var headingLabel = headerTemplate.GetNode<Label>("Heading");
        var timestampLabel = headerTemplate.GetNode<Label>("Timestamp");

        headingLabel.Text = message.Heading;
        timestampLabel.Text = message.Timestamp;
    }

    private void UpdateLines(VBoxContainer messageTemplate, GameLogMessage message) {
        if (_lineTemplateNode is null) {
            GD.PrintErr("LineTemplate node is not assigned.");
            return;
        }

        var linesContainer = messageTemplate.GetNode<VBoxContainer>("LinesContainer");

        // Clear existing lines
        foreach (Node child in linesContainer.GetChildren()) {
            if (child != _lineTemplateNode) {
                child.QueueFree();
            }
        }

        foreach (var line in message.Lines) {
            var lineLabel = (RichTextLabel)_lineTemplateNode.Duplicate();
            lineLabel.Visible = true;
            lineLabel.BbcodeEnabled = true;
            lineLabel.Text = "";
            lineLabel.AppendText($"[color={GetColorForLineType(line.Type)}]{line.Text}[/color]");
            linesContainer.AddChild(lineLabel);
        }
    }

    private void ClearTemplateTexts(VBoxContainer messageTemplate) {
        var headerTemplate = messageTemplate.GetNode<HBoxContainer>("HeaderTemplate");
        var headingLabel = headerTemplate.GetNode<Label>("Heading");
        var timestampLabel = headerTemplate.GetNode<Label>("Timestamp");
        var linesContainer = messageTemplate.GetNode<VBoxContainer>("LinesContainer");
        var lineLabel = linesContainer.GetNode<RichTextLabel>("LineTemplate");

        headingLabel.Text = "";
        timestampLabel.Text = "";
        lineLabel.Text = "";
    }

    private static string GetColorForLineType(GameLogLineType type) {
        return type switch {
            GameLogLineType.Default => "white",
            GameLogLineType.Error => "red",
            GameLogLineType.Info => "blue",
            GameLogLineType.Success => "green",
            GameLogLineType.Warning => "yellow",
            GameLogLineType.Wip => "gray",
            _ => "white",
        };
    }
}
