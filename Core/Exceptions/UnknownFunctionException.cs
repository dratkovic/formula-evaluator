namespace Core.Exceptions;

public class UnknownFunctionException : FormulaParseException
{
    public UnknownFunctionException(string keyword)
        : base($"Unknown function: '{keyword}'")
    {
    }
}
