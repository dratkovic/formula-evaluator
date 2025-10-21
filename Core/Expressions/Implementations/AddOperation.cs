namespace Core.Expressions.Implementations;

public class AddOperation : BinaryOperation
{
    public AddOperation(string input) : base(input) { }

    public override string Keyword => "add";

    public override double Resolve()
    {
        return Parameter1.Resolve() + Parameter2.Resolve();
    }
}
