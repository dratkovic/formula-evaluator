namespace Core.Expressions.Implementations;

public class MultiplyOperation : BinaryOperation
{
    public MultiplyOperation(string input) : base(input) { }

    public override string Keyword => "multiply";

    public override double Resolve()
    {
        return Parameter1.Resolve() * Parameter2.Resolve();
    }
}
