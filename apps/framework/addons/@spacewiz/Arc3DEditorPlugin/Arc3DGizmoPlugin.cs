using Godot;
using DiceRolling.Helpers;

namespace DiceRolling.Editor;

/// <summary>
/// A plugin for rendering 3D gizmos in the editor for Arc3DRenderer nodes.
/// </summary>
[Tool]
public partial class Arc3DGizmoPlugin : EditorNode3DGizmoPlugin
{
    private readonly StandardMaterial3D _redMaterial;
    private readonly StandardMaterial3D _greenMaterial;
    private readonly StandardMaterial3D _blueMaterial;

    private const float SphereRadius = 0.15f;
    private const float SphereHeight = 0.25f;

    /// <summary>
    /// Initializes a new instance of the <see cref="Arc3DGizmoPlugin"/> class.
    /// </summary>
    public Arc3DGizmoPlugin()
    {
        _redMaterial = CreateMaterial(Colors.Red);
        _greenMaterial = CreateMaterial(Colors.Green);
        _blueMaterial = CreateMaterial(Colors.Blue);
    }

    /// <summary>
    /// Creates a new material with the specified color.
    /// </summary>
    /// <param name="color">The color of the material.</param>
    /// <returns>A new <see cref="StandardMaterial3D"/> with the specified color.</returns>
    private StandardMaterial3D CreateMaterial(Color color)
    {
        return new StandardMaterial3D
        {
            AlbedoColor = color
        };
    }

    /// <summary>
    /// Gets the name of the gizmo.
    /// <inheritdoc cref="Godot.EditorNode3DGizmoPlugin._GetGizmoName"/>
    /// </summary>
    /// <returns>The name of the gizmo.</returns>
    public override string _GetGizmoName()
    {
        return "@spacewiz Arc3D Gizmo";
    }

    /// <summary>
    /// Redraws the gizmo for the specified <see cref="EditorNode3DGizmo"/>.
    /// <inheritdoc cref="Godot.EditorNode3DGizmoPlugin._Redraw"/>
    /// </summary>
    /// <param name="gizmo">The gizmo to redraw.</param>
    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        if (gizmo.GetNode3D() is not Arc3DRenderer arcRenderer)
            return;

        gizmo.Clear();

        // Draw lines between points
        gizmo.AddLines([arcRenderer.PointA, arcRenderer.PointB], _redMaterial);
        gizmo.AddLines([arcRenderer.PointB, arcRenderer.PointC], _greenMaterial);

        // Draw spheres at points
        var sphereMesh = new SphereMesh { Radius = SphereRadius, Height = SphereHeight };
        AddSphere(gizmo, sphereMesh, arcRenderer.PointA, _redMaterial);
        AddSphere(gizmo, sphereMesh, arcRenderer.PointB, _greenMaterial);
        AddSphere(gizmo, sphereMesh, arcRenderer.PointC, _blueMaterial);
    }

    /// <summary>
    /// Adds a sphere to the gizmo at the specified position.
    /// </summary>
    /// <param name="gizmo">The gizmo to add the sphere to.</param>
    /// <param name="sphereMesh">The mesh of the sphere.</param>
    /// <param name="position">The position of the sphere.</param>
    /// <param name="material">The material of the sphere.</param>
    private static void AddSphere(EditorNode3DGizmo gizmo, SphereMesh sphereMesh, Vector3 position, StandardMaterial3D material)
    {
        var transform = new Transform3D(Basis.Identity, position);
        gizmo.AddMesh(sphereMesh, material, transform);
    }

    /// <summary>
    /// Determines whether the specified node has a gizmo.
    /// <inheritdoc cref="Godot.EditorNode3DGizmoPlugin._HasGizmo"/>
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns><c>true</c> if the node has a gizmo; otherwise, <c>false</c>.</returns>
    public override bool _HasGizmo(Node3D node)
    {
        return node is Arc3DRenderer;
    }
}