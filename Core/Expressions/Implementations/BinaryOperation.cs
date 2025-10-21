namespace Core.Expressions.Implementations;

public abstract class BinaryOperation : ExpressionBase
{
    public override OperationType Type => OperationType.Binary;

    protected BinaryOperation(string input)
    {
        // Extract content between parentheses
        var content = StringParser.ExtractParametersContent(input);

        // Split into parameters
        var parameters = StringParser.SplitParameters(content);

        // Validate parameter count
        StringParser.ValidateParameterCount(this.Keyword!, 2, parameters.Count);

        // Recursively build child expressions
        Parameter1 = ExpressionBase.Build(parameters[0]);
        Parameter2 = ExpressionBase.Build(parameters[1]);
    }
}
