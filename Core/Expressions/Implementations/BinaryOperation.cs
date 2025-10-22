namespace Core.Expressions.Implementations;

public abstract class BinaryOperation : ExpressionBase
{
    public override OperationType Type => OperationType.Binary;

    protected BinaryOperation(string input)
    {
        // Extract keyword for validation error messages
        var keyword = StringParser.ExtractKeyword(input);

        // Extract content between parentheses
        var content = StringParser.ExtractParametersContent(input);

        // Split into parameters
        var parameters = StringParser.SplitParameters(content);

        // Validate parameter count
        StringParser.ValidateParameterCount(keyword, GetExpectedParameterCount(Type), parameters.Count);

        // Recursively build child expressions
        Parameter1 = Build(parameters[0]);
        Parameter2 = Build(parameters[1]);
    }
}
