using Godot;

namespace DiceRolling.Components;

/// <summary>
/// Smoothly interpolates a Camera3D's position and optionally rotation
/// between its initial state and a randomized target state within defined ranges,
/// looping the movement. Useful for testing views.
/// </summary>
[GlobalClass]
public partial class CameraMovement : Node { // Attach this script as a child of Camera3D

    [ExportGroup("Movement")]
    [Export] public Vector3 PositionRandomRange { get; set; } = new Vector3(5, 2, 5);
    [Export] public double MoveDuration { get; set; } = 2.0; // Duration for one way movement
    [Export] public double DelayAtEnds { get; set; } = 1.0; // Optional delay at start and target positions

    [ExportGroup("Rotation (Optional)")]
    [Export] public bool RandomizeRotation { get; set; } = false;
    [Export(PropertyHint.Range, "0,180,0.1")] public Vector3 RotationRandomRangeDegrees { get; set; } = new Vector3(10, 20, 0);

    private Camera3D? _camera;
    private Vector3 _initialPosition;
    private Vector3 _initialRotationDegrees;
    private Tween? _moveTween;
    private RandomNumberGenerator _rng = new();

    public override void _Ready() {
        _camera = GetParent<Camera3D>();
        if (_camera == null) {
            GD.PrintErr("[CameraMoveLoopComponent] Parent must be a Camera3D.");
            SetProcess(false);
            SetPhysicsProcess(false);
            return;
        }
        _initialPosition = _camera.GlobalPosition;
        _initialRotationDegrees = _camera.RotationDegrees;
        StartNextMove(); // Start the first move
    }

    private void StartNextMove() {
        if (_camera == null) return;

        // --- Calculate Random Target ---
        Vector3 randomPositionOffset = new Vector3(
            _rng.RandfRange(-PositionRandomRange.X, PositionRandomRange.X),
            _rng.RandfRange(-PositionRandomRange.Y, PositionRandomRange.Y),
            _rng.RandfRange(-PositionRandomRange.Z, PositionRandomRange.Z)
        );
        Vector3 targetPosition = _initialPosition + randomPositionOffset;

        Vector3 targetRotationDegrees = _initialRotationDegrees; // Default to initial
        if (RandomizeRotation) {
            Vector3 randomRotationOffset = new Vector3(
                _rng.RandfRange(-RotationRandomRangeDegrees.X, RotationRandomRangeDegrees.X),
                _rng.RandfRange(-RotationRandomRangeDegrees.Y, RotationRandomRangeDegrees.Y),
                _rng.RandfRange(-RotationRandomRangeDegrees.Z, RotationRandomRangeDegrees.Z)
            );
            targetRotationDegrees = _initialRotationDegrees + randomRotationOffset;
        }
        // --- End Calculate Random Target ---


        _moveTween?.Kill(); // Ensure previous tween is stopped if any

        _moveTween = CreateTween();
        _moveTween.SetTrans(Tween.TransitionType.Sine); // Use Sine for smoother start/end
        _moveTween.SetEase(Tween.EaseType.InOut);      // Ease in and out

        // --- Sequence ---
        // Move To Target
        var tweenPropPos = _moveTween.TweenProperty(_camera, "global_position", targetPosition, MoveDuration);
        if (RandomizeRotation) {
            // Add rotation in parallel if enabled
            _moveTween.Parallel().TweenProperty(_camera, "rotation_degrees", targetRotationDegrees, MoveDuration);
        }

        // Delay at Target
        if (DelayAtEnds > 0) {
            _moveTween.TweenInterval(DelayAtEnds);
        }

        // Move Back to Initial
        tweenPropPos = _moveTween.TweenProperty(_camera, "global_position", _initialPosition, MoveDuration);
        if (RandomizeRotation) {
            // Add rotation back in parallel if enabled
            _moveTween.Parallel().TweenProperty(_camera, "rotation_degrees", _initialRotationDegrees, MoveDuration);
        }

        // Delay at Initial
        if (DelayAtEnds > 0) {
            _moveTween.TweenInterval(DelayAtEnds);
        }

        // --- Loop by starting the next move ---
        _moveTween.TweenCallback(Callable.From(StartNextMove));
    }

    public override void _ExitTree() {
        _moveTween?.Kill(); // Clean up tween when the node exits
    }
}