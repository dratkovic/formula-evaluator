namespace Core.Expressions.Implementations;

[ExpressionKeyword("divide")]
public class DivideOperation : BinaryOperation
{
    public DivideOperation(string input) : base(input) { }

    public override double Resolve()
    {
        var divisor = Parameter2.Resolve();
        if (Math.Abs(divisor) < double.Epsilon)
            throw new DivideByZeroException("Cannot divide by zero");

        return Parameter1.Resolve() / divisor;
    }
}
