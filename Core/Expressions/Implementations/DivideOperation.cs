namespace Core.Expressions.Implementations;

public class DivideOperation : BinaryOperation
{
    public DivideOperation(string input) : base(input) { }

    public override string Keyword => "divide";

    public override double Resolve()
    {
        var divisor = Parameter2.Resolve();
        if (Math.Abs(divisor) < double.Epsilon)
            throw new DivideByZeroException("Cannot divide by zero");

        return Parameter1.Resolve() / divisor;
    }
}
