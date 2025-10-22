namespace Core.Expressions.Implementations;

[ExpressionKeyword("subtract")]
public class SubtractOperation : BinaryOperation
{
    public SubtractOperation(string input) : base(input) { }

    public override double Resolve()
    {
        return Parameter1.Resolve() - Parameter2.Resolve();
    }
}
