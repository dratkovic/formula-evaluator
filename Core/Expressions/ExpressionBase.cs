using System.Linq.Expressions;
using System.Reflection;
using Core.Exceptions;
using Core.Expressions.Implementations;

namespace Core.Expressions;

public abstract class ExpressionBase
{
    protected ExpressionBase Parameter1 { get; set; } = null!;
    protected ExpressionBase Parameter2 { get; set; } = null!;
    protected ExpressionBase Parameter3 { get; set; } = null!;

    private static readonly Dictionary<string, Func<string, ExpressionBase>> _expressionFactories;

    public abstract double Resolve();
    public abstract OperationType Type { get; }

    /// <summary>
    /// Main entry point for building expression trees from string formulas
    /// </summary>
    public static ExpressionBase Build(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new FormulaParseException("Input cannot be empty or whitespace");

        input = input.Trim();

        // Check if it's a number - let NumberPrimitive handle validation
        if (double.TryParse(input, out _))
            return new NumberPrimitive(input);

        // Extract keyword
        var keyword = StringParser.ExtractKeyword(input);

        // Validate keyword exists
        if (!_expressionFactories.ContainsKey(keyword))
            throw new UnknownFunctionException(keyword);

        // Use compiled factory delegate for fast instantiation
        var factory = _expressionFactories[keyword];

        try
        {
            return factory(input);
        }
        catch (TargetInvocationException ex) when (ex.InnerException != null)
        {
            // Unwrap exceptions thrown by constructors
            throw ex.InnerException;
        }
    }

    /// <summary>
    /// Static constructor - uses reflection to discover all expression implementations
    /// and compile fast factory delegates. Thread-safe initialization guaranteed by CLR.
    /// </summary>
    static ExpressionBase()
    {
        _expressionFactories = new Dictionary<string, Func<string, ExpressionBase>>();

        var assembly = typeof(ExpressionBase).Assembly;
        var expressionTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ExpressionBase)) && t != typeof(NumberPrimitive));

        foreach (var type in expressionTypes)
        {
            try
            {
                // Get keyword from attribute instead of instantiating
                var attribute = type.GetCustomAttribute<ExpressionKeywordAttribute>();
                if (attribute == null)
                    continue;

                var keyword = attribute.Keyword;

                // Create compiled factory delegate for fast instantiation
                var constructor = type.GetConstructor(new[] { typeof(string) });
                if (constructor == null)
                    continue;

                // Build expression: (string input) => new TExpression(input)
                var inputParam = Expression.Parameter(typeof(string), "input");
                var newExpr = Expression.New(constructor, inputParam);
                var castExpr = Expression.Convert(newExpr, typeof(ExpressionBase));
                var lambda = Expression.Lambda<Func<string, ExpressionBase>>(castExpr, inputParam);

                // Compile to delegate - much faster than Activator.CreateInstance
                _expressionFactories[keyword] = lambda.Compile();
            }
            catch
            {
                // Skip types that can't be processed
            }
        }
    }

    protected static int GetExpectedParameterCount(OperationType type)
    {
        return type switch
        {
            OperationType.Primitive => 0,
            OperationType.Unary => 1,
            OperationType.Binary => 2,
            OperationType.Ternary => 3,
            _ => throw new ArgumentException($"Unknown operation type: {type}")
        };
    }
}