namespace Core.Expressions.Implementations;

[ExpressionKeyword("add")]
public class AddOperation : BinaryOperation
{
    public AddOperation(string input) : base(input) { }

    public override double Resolve()
    {
        return Parameter1.Resolve() + Parameter2.Resolve();
    }
}
