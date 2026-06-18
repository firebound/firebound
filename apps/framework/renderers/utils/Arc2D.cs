using Godot;

namespace DiceRolling.Helpers;

[Tool]
public partial class Arc2D : Line2D
{
    private int _bendStrength = 10;
    [Export]
    public int BendStrength
    {
        get => _bendStrength;
        set
        {
            _bendStrength = value;
            SetupArcStatic();
        }
    }

    private string _bendDirection = "Right";
    [Export]
    public string BendDirection
    {
        get => _bendDirection;
        set
        {
            _bendDirection = value;
            SetupArcStatic();
        }
    }

    private int _smoothness = 10;
    [Export]
    public int Smoothness
    {
        get => _smoothness;
        set
        {
            _smoothness = value;
            SetupArcStatic();
        }
    }

    private Vector2 _start;
    private Vector2 _end;
    private Vector2 _midpoint;
    private Vector2 _arcMidpoint;

    private Vector2 _lastStartPosition;
    private Vector2 _lastEndPosition;

    public override void _Process(double delta)
    {
        SetupArcFlexible();
    }

    private bool IsPositionTheSame()
    {
        return _start == _lastStartPosition && _end == _lastEndPosition;
    }

    private void SetupArcStatic()
    {
        SetExtremePoints();
        ResetPoints();
        SetMidpoints();
        SetArcSegments();
    }

    private void SetupArcFlexible()
    {
        if (Points.Length < 2) // Ensure Points.Length is used as a property
            return;

        SetExtremePoints();

        if (IsPositionTheSame())
            return;

        ResetPoints();
        SetMidpoints();
        SetArcSegments();
    }

    private void SetExtremePoints()
    {
        _start = Points[0];
        _end = Points[Points.Length - 1];
    }

    private void ResetPoints()
    {
        _lastStartPosition = _start;
        _lastEndPosition = _end;

        ClearPoints();
    }

    private void SetMidpoints()
    {
        _midpoint = (_start + _end) / 2;
        _arcMidpoint = _midpoint + GetBendDirection();
    }

    private Vector2 GetBendDirection()
    {
        if (BendDirection == "Left")
            return new Vector2(_start.Y - _midpoint.Y, _midpoint.X - _start.X).Normalized() * BendStrength * 5;

        return new Vector2(_midpoint.Y - _start.Y, _start.X - _midpoint.X).Normalized() * BendStrength * 5;
    }

    private void SetArcSegments()
    {
        AddPoint(_start, 0);

        for (int segment = 1; segment < Smoothness - 1; segment++)
        {
            AddPoint(InterpolateArcPoint((float)segment / (Smoothness - 1)), segment);
        }

        AddPoint(_end, Smoothness - 1);
    }

    private Vector2 InterpolateArcPoint(float segment)
    {
        Vector2 interpolatedPoint1 = _start.Lerp(_arcMidpoint, segment);
        Vector2 interpolatedPoint2 = _arcMidpoint.Lerp(_end, segment);

        return interpolatedPoint1.Lerp(interpolatedPoint2, segment);
    }
}