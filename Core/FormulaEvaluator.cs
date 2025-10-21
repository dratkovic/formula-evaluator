using Core.Exceptions;
using Core.Expressions;

namespace Core;

public class FormulaEvaluator
{
    /// <summary>
    /// Evaluates a mathematical formula expressed as a string
    /// </summary>
    /// <param name="formula">The formula to evaluate (e.g., "add(2, 3)", "multiply(add(2, 3), 4)")</param>
    /// <returns>The computed result</returns>
    public string Evaluate(string formula, int decimalPlaces = 2)
    {
        try
        {
            var expression = ExpressionBase.Build(formula);
            var result = expression.Resolve();
            return Math.Round(result, decimalPlaces).ToString();
        }
        catch (FormulaParseException ex)
        {
            return $"Error while parsing formula: {ex.Message}";
        }
        catch (DivideByZeroException)
        {
            return "Error: Division by zero";
        }
        catch (Exception ex)
        {
            return $"Unexpected error: {ex.Message}";
        }
    }
}
