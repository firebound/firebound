using System;
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using DiceRolling.Attributes;

namespace DiceRolling.Tests.Attributes;

[TestSuite]
public class AttributeTypeTests {
    [TestCase]
    public static void Constructor_ShouldInitializeProperties() {
        // Arrange
        var name = "Strength";
        var description = "Determines physical power.";
        var color = new Color(1, 0, 0);
        var icon = new Texture2D();
        var minValue = 1;
        var maxValue = 10;

        // Act
        var attribute = new AttributeType(name, description, color, icon, minValue, maxValue);

        // Assert
        AssertString(attribute.Name).IsEqual(name);
        AssertString(attribute.Description).IsEqual(description);
        AssertObject(attribute.Color).IsEqual(color);
        AssertObject(attribute.Icon).IsEqual(icon);
        AssertInt(attribute.MinValue).IsEqual(minValue);
        AssertInt(attribute.MaxValue).IsEqual(maxValue);
    }
}