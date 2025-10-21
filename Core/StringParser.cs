using System.Text;
using Core.Exceptions;

namespace Core;

public static class StringParser
{
    /// <summary>
    /// Extract function name from input like "add(2, 3)" returns "add"
    /// </summary>
    public static string ExtractKeyword(string input)
    {
        var openParenIndex = input.IndexOf('(');
        if (openParenIndex == -1)
            throw new FormulaParseException($"Missing opening parenthesis in: '{input}'");

        var keyword = input.Substring(0, openParenIndex).Trim();

        if (string.IsNullOrWhiteSpace(keyword))
            throw new FormulaParseException($"Missing function name in: '{input}'");

        return keyword;
    }

    /// <summary>
    /// Extract content between parentheses from "add(2, 3)" returns "2, 3"
    /// </summary>
    public static string ExtractParametersContent(string input)
    {
        var openParenIndex = input.IndexOf('(');
        var closeParenIndex = input.LastIndexOf(')');

        if (openParenIndex == -1)
            throw new FormulaParseException($"Missing opening parenthesis in: '{input}'");

        if (closeParenIndex == -1)
            throw new FormulaParseException($"Missing closing parenthesis in: '{input}'");

        if (closeParenIndex < openParenIndex)
            throw new FormulaParseException($"Mismatched parentheses in: '{input}'");

        // Extract content between first '(' and last ')'
        var content = input.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);
        return content.Trim();
    }

    /// <summary>
    /// Split parameters by commas at the same nesting level.
    /// Handles nested parentheses correctly using bracket counting.
    /// Example: "2, multiply(3, 4)" returns ["2", "multiply(3, 4)"]
    /// </summary>
    public static List<string> SplitParameters(string parametersContent)
    {
        var parameters = new List<string>();
        var currentParam = new StringBuilder();
        var depth = 0;

        for (int i = 0; i < parametersContent.Length; i++)
        {
            var ch = parametersContent[i];

            if (ch == '(')
            {
                depth++;
                currentParam.Append(ch);
            }
            else if (ch == ')')
            {
                depth--;
                currentParam.Append(ch);

                if (depth < 0)
                    throw new FormulaParseException("Mismatched parentheses: too many closing parentheses");
            }
            else if (ch == ',' && depth == 0)
            {
                // Found parameter separator at same nesting level
                parameters.Add(currentParam.ToString().Trim());
                currentParam.Clear();
            }
            else
            {
                currentParam.Append(ch);
            }
        }

        // Add last parameter
        if (currentParam.Length > 0)
        {
            parameters.Add(currentParam.ToString().Trim());
        }

        if (depth != 0)
            throw new FormulaParseException("Mismatched parentheses: unclosed opening parentheses");

        return parameters;
    }

    /// <summary>
    /// Validate that the actual parameter count matches the expected count
    /// </summary>
    public static void ValidateParameterCount(string functionName, int expected, int actual)
    {
        if (expected != actual)
            throw new InvalidParameterCountException(functionName, expected, actual);
    }
}
