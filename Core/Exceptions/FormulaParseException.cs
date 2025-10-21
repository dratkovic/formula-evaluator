namespace Core.Exceptions;

public class FormulaParseException : Exception
{
    public FormulaParseException(string message) : base(message) { }

    public FormulaParseException(string message, Exception innerException)
        : base(message, innerException) { }
}
