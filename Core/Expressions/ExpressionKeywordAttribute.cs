namespace Core.Expressions;

/// <summary>
/// Attribute to specify the keyword for an expression type without requiring instantiation
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ExpressionKeywordAttribute : Attribute
{
    public string Keyword { get; }

    public ExpressionKeywordAttribute(string keyword)
    {
        Keyword = keyword;
    }
}
