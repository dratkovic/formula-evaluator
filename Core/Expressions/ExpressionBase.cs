using System.Reflection;
using System.Text.RegularExpressions;
using Core.Exceptions;
using Core.Expressions.Implementations;

namespace Core.Expressions;

public abstract class ExpressionBase
{
    protected ExpressionBase Parameter1 { get; set; } = null!;
    protected ExpressionBase Parameter2 { get; set; } = null!;
    protected ExpressionBase Parameter3 { get; set; } = null!;

    private static readonly Dictionary<string, Type> _expressionTypes;
    private static readonly Regex _numberRegex = new Regex(@"^-?\d+(\.\d+)?$");

    public abstract string? Keyword { get; }
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

        // Check if it's a number
        if (_numberRegex.IsMatch(input))
            return new NumberPrimitive(input);

        // Extract keyword
        var keyword = StringParser.ExtractKeyword(input);

        // Validate keyword exists
        if (!_expressionTypes.ContainsKey(keyword))
            throw new UnknownFunctionException(keyword);

        var expressionType = _expressionTypes[keyword];

        // Create instance with input string
        try
        {
            return (ExpressionBase)Activator.CreateInstance(expressionType, input)!;
        }
        catch (TargetInvocationException ex) when (ex.InnerException != null)
        {
            // Unwrap exceptions thrown by constructors
            throw ex.InnerException;
        }
    }

    /// <summary>
    /// Static constructor - uses reflection to discover all expression implementations
    /// Thread-safe initialization guaranteed by CLR
    /// </summary>
    static ExpressionBase()
    {
        _expressionTypes = new Dictionary<string, Type>();

        var assembly = typeof(ExpressionBase).Assembly;
        var expressionTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(ExpressionBase)) && t != typeof(NumberPrimitive));

        foreach (var type in expressionTypes)
        {
            try
            {
                // Create a temporary instance to get the keyword
                // We need to handle the fact that constructors now expect a string
                var constructor = type.GetConstructor(new[] { typeof(string) });
                if (constructor != null)
                {
                    // Use a dummy valid expression to get the keyword
                    var dummyInput = GetDummyInputForType(type);
                    var instance = (ExpressionBase)constructor.Invoke(new object[] { dummyInput });

                    if (instance.Keyword != null)
                    {
                        _expressionTypes[instance.Keyword] = type;
                    }
                }
            }
            catch
            {
                // Skip types that can't be instantiated
            }
        }
    }

    private static string GetDummyInputForType(Type type)
    {
        // Get the keyword from a static property if available, otherwise use reflection on the property
        var keywordProperty = type.GetProperty("Keyword");
        if (keywordProperty != null)
        {
            // For binary operations, we need a valid dummy expression
            return "dummy(0, 0)";
        }
        return "0";
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