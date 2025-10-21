namespace Core.Expressions.Implementations;

public class SubtractOperation : BinaryOperation
{
    public SubtractOperation(string input) : base(input) { }

    public override string Keyword => "subtract";

    public override double Resolve()
    {
        return Parameter1.Resolve() - Parameter2.Resolve();
    }
}
