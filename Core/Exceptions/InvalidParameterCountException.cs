namespace Core.Exceptions;

public class InvalidParameterCountException : FormulaParseException
{
    public InvalidParameterCountException(string functionName, int expected, int actual)
        : base($"Function '{functionName}' expects {expected} parameters but got {actual}")
    {
    }
}
