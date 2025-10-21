using Core.Exceptions;

namespace Core.Expressions.Implementations;

public class NumberPrimitive : ExpressionBase
{
    private readonly double _value;

    public NumberPrimitive(string input)
    {
        if (!double.TryParse(input.Trim(), out _value))
            throw new FormulaParseException($"Invalid number format: '{input}'");
    }

    public override string? Keyword => null;

    public override OperationType Type => OperationType.Primitive;

    public override double Resolve() => _value;
}
