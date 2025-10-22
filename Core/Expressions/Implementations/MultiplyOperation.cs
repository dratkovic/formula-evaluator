namespace Core.Expressions.Implementations;

[ExpressionKeyword("multiply")]
public class MultiplyOperation : BinaryOperation
{
    public MultiplyOperation(string input) : base(input) { }

    public override double Resolve()
    {
        return Parameter1.Resolve() * Parameter2.Resolve();
    }
}
