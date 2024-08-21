using System.Text.RegularExpressions;

namespace CombatManager.Utils.Validation;

public partial class InputValidator
{
    public static bool ValidateNumOnlyInput(string originalText, string newText)
    {
        // ToDo: This currently allows a - to be put at the end of the text.
        var regex = NumInputRegex();
        return regex.IsMatch(newText + originalText);
    }
    
    [GeneratedRegex(@"^[-]?[0-9]+")]
    private static partial Regex NumInputRegex();
}