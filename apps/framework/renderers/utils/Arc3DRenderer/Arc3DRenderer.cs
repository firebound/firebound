using Godot;

namespace DiceRolling.Helpers;

/// <summary>
/// A class for generating a 3D path and placing cubes along it.
/// </summary>
[Tool]
public partial class Arc3DRenderer : Node3D
{
    /// <summary>
    /// Gets or sets the first point of the quadratic Bezier curve.
    /// </summary>
    [Export] public Vector3 PointA { get; set; } = new Vector3(0, 0, 0);

    /// <summary>
    /// Gets or sets the second point (control point) of the quadratic Bezier curve.
    /// </summary>
    [Export] public Vector3 PointB { get; set; } = new Vector3(5, 5, 0);

    /// <summary>
    /// Gets or sets the third point of the quadratic Bezier curve.
    /// </summary>
    [Export] public Vector3 PointC { get; set; } = new Vector3(10, 0, 0);

    /// <summary>
    /// Gets or sets the size of the cubes to be placed along the path.
    /// </summary>
    [Export] public Vector3 CubeSize { get; set; } = new Vector3(0.5f, 0.5f, 0.5f);

    /// <summary>
    /// Gets or sets the spacing between the cubes along the path.
    /// </summary>
    [Export] public float Spacing { get; set; } = 1.0f;

    /// <summary>
    /// Gets the callable for the generate grid button.
    /// </summary>
    [ExportToolButton("Generate")]
    private Callable GenerateGridButton => Callable.From(GeneratePath);

    private Path3D? _path;
    private Curve3D? _curve;

    /// <summary>
    /// Called when the node is added to the scene.
    /// <inheritdoc cref="Godot.Node._Ready"/>
    /// </summary>
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            SetupPath();
        }
    }

    /// <summary>
    /// Generates the path and places cubes along it.
    /// </summary>
    public void GeneratePath()
    {
        ClearPath();
        SetupPath();
    }

    /// <summary>
    /// Clears the existing path.
    /// </summary>
    private void ClearPath()
    {
        _path?.QueueFree();
    }

    /// <summary>
    /// Sets up the path and places cubes along it.
    /// </summary>
    private void SetupPath()
    {
        _path = new Path3D();
        _curve = new Curve3D();

        int segments = 20;
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector3 point = CalculateQuadraticBezierPoint(t, PointA, PointB, PointC);
            _curve.AddPoint(point);
        }

        _path.Curve = _curve;
        AddChild(_path);

        float pathLength = _curve.GetBakedLength();
        int cubeCount = Mathf.FloorToInt(pathLength / Spacing);

        for (int i = 0; i < cubeCount; i++)
        {
            var pathFollow = new PathFollow3D
            {
                CubicInterp = true,
                RotationMode = PathFollow3D.RotationModeEnum.Xyz,
                Loop = true
            };
            _path.AddChild(pathFollow);

            var meshInstance = new MeshInstance3D
            {
                Mesh = new BoxMesh { Size = CubeSize }
            };
            pathFollow.AddChild(meshInstance);

            pathFollow.ProgressRatio = (float)i / (cubeCount - 1);
        }
    }

    /// <summary>
    /// Calculates a point on a quadratic Bezier curve.
    /// </summary>
    /// <param name="t">The parameter t, where 0 ≤ t ≤ 1.</param>
    /// <param name="a">The first point of the curve.</param>
    /// <param name="b">The second point of the curve.</param>
    /// <param name="c">The third point of the curve.</param>
    /// <returns>The calculated point on the curve.</returns>
    private static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 a, Vector3 b, Vector3 c)
    {
        return (1 - t) * (1 - t) * a + 2 * (1 - t) * t * b + t * t * c;
    }
}