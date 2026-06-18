using Godot;

namespace DiceRolling.Services;

public class ValidationService {
    private static ValidationService? _instance;
    public static ValidationService Instance => _instance ??= new ValidationService();

    private ValidationService() { }
    public static bool ValidateName(string? value) {
        var isValid = !string.IsNullOrWhiteSpace(value);
        if (!isValid) {
            GD.PrintErr("Value cannot be null or whitespace");
        }
        return isValid;
    }

    public static bool ValidateId(string? value) {
        var isValid = !string.IsNullOrEmpty(value);
        if (!isValid) {
            GD.PrintErr("Value cannot be null or empty");
        }
        return isValid;
    }

    public static bool ValidateMinMaxValues(int minValue, int maxValue) {
        var isValid = minValue <= maxValue;
        if (!isValid) {
            GD.PrintErr("MinValue must be less than or equal to MaxValue");
        }
        return isValid;
    }

    public static bool ValidateGridDimensions(int rows, int columns) {
        var isValid = rows > 0 && columns > 0;
        if (!isValid) {
            GD.PrintErr("Rows and Columns must be greater than 0.");
        }
        return isValid;
    }
}