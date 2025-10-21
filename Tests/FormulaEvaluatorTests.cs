using AwesomeAssertions;
using Core;
using Xunit;

namespace Tests;

public class FormulaEvaluatorTests
{
    private readonly FormulaEvaluator _evaluator = new();

    #region Happy Path Tests

    [Fact]
    public void Evaluate_SimpleNumber_ReturnsValue()
    {
        var result = _evaluator.Evaluate("42");
        result.Should().Be("42");
    }

    [Fact]
    public void Evaluate_NegativeNumber_ReturnsValue()
    {
        var result = _evaluator.Evaluate("-5");
        result.Should().Be("-5");
    }

    [Fact]
    public void Evaluate_DecimalNumber_ReturnsValue()
    {
        var result = _evaluator.Evaluate("3.14");
        result.Should().Be("3.14");
    }

    [Fact]
    public void Evaluate_SimpleAdd_ReturnsSum()
    {
        var result = _evaluator.Evaluate("add(2, 3)");
        result.Should().Be("5");
    }

    [Fact]
    public void Evaluate_SimpleSubtract_ReturnsDifference()
    {
        var result = _evaluator.Evaluate("subtract(10, 4)");
        result.Should().Be("6");
    }

    [Fact]
    public void Evaluate_SimpleMultiply_ReturnsProduct()
    {
        var result = _evaluator.Evaluate("multiply(3, 4)");
        result.Should().Be("12");
    }

    [Fact]
    public void Evaluate_SimpleDivide_ReturnsQuotient()
    {
        var result = _evaluator.Evaluate("divide(8, 2)");
        result.Should().Be("4");
    }

    [Fact]
    public void Evaluate_NestedAddMultiply_ReturnsCorrectValue()
    {
        var result = _evaluator.Evaluate("add(2, multiply(3, 4))");
        result.Should().Be("14");
    }

    [Fact]
    public void Evaluate_NestedMultipleOperations_ReturnsCorrectValue()
    {
        var result = _evaluator.Evaluate("multiply(add(2, 3), divide(10, 2))");
        result.Should().Be("25");
    }

    [Fact]
    public void Evaluate_DeepNesting_ReturnsCorrectValue()
    {
        var result = _evaluator.Evaluate("add(1, add(2, add(3, 4)))");
        result.Should().Be("10");
    }

    [Fact]
    public void Evaluate_WithWhitespace_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("add( 2 , 3 )");
        result.Should().Be("5");
    }

    [Fact]
    public void Evaluate_ComplexNestedExpression_ReturnsCorrectValue()
    {
        var result = _evaluator.Evaluate("add(2, multiply(2, add(2, 2)))");
        result.Should().Be("10");
    }

    [Fact]
    public void Evaluate_VeryComplexNesting_ReturnsCorrectValue()
    {
        var result = _evaluator.Evaluate("add(2, multiply(add(1, multiply(1, 1)), divide(2, 2)))");
        result.Should().Be("4");
    }

    [Fact]
    public void Evaluate_DecimalPlacesDefault_RoundsToTwoPlaces()
    {
        var result = _evaluator.Evaluate("divide(10, 3)");
        result.Should().Be("3.33");
    }

    [Fact]
    public void Evaluate_DecimalPlacesCustom_RoundsCorrectly()
    {
        var result = _evaluator.Evaluate("divide(10, 3)", 4);
        result.Should().Be("3.3333");
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public void Evaluate_EmptyString_ReturnsError()
    {
        var result = _evaluator.Evaluate("");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_WhitespaceOnly_ReturnsError()
    {
        var result = _evaluator.Evaluate("   ");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_InvalidNumber_ReturnsError()
    {
        var result = _evaluator.Evaluate("abc");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_UnknownFunction_ReturnsError()
    {
        var result = _evaluator.Evaluate("unknown(2, 3)");
        result.Should().StartWith("Error while parsing formula:").And.Contain("Unknown function");
    }

    [Fact]
    public void Evaluate_MissingOpenParenthesis_ReturnsError()
    {
        var result = _evaluator.Evaluate("add 2, 3)");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_MissingCloseParenthesis_ReturnsError()
    {
        var result = _evaluator.Evaluate("add(2, 3");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_MismatchedParentheses_ReturnsError()
    {
        var result = _evaluator.Evaluate("add(2, 3))");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_TooFewParameters_ReturnsError()
    {
        var result = _evaluator.Evaluate("add(2)");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_TooManyParameters_ReturnsError()
    {
        var result = _evaluator.Evaluate("add(2, 3, 4)");
        result.Should().StartWith("Error while parsing formula:");
    }

    [Fact]
    public void Evaluate_DivideByZero_ReturnsError()
    {
        var result = _evaluator.Evaluate("divide(10, 0)");
        result.Should().Be("Error: Division by zero");
    }

    [Fact]
    public void Evaluate_DivideByZeroNested_ReturnsError()
    {
        var result = _evaluator.Evaluate("add(2, divide(10, 0))");
        result.Should().Be("Error: Division by zero");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Evaluate_NegativeNumbers_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("add(-2, 3)");
        result.Should().Be("1");
    }

    [Fact]
    public void Evaluate_NegativeResult_ReturnsNegativeValue()
    {
        var result = _evaluator.Evaluate("subtract(5, 10)");
        result.Should().Be("-5");
    }

    [Fact]
    public void Evaluate_DecimalNumbers_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("multiply(2.5, 4)");
        result.Should().Be("10");
    }

    [Fact]
    public void Evaluate_VeryLargeNumber_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("1000000");
        result.Should().Be("1000000");
    }

    [Fact]
    public void Evaluate_VerySmallNumber_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("0.0001");
        result.Should().Be("0");
    }

    [Fact]
    public void Evaluate_VerySmallNumber_WithPrecision_HandlesCorrectly()
    {
        var result = _evaluator.Evaluate("0.0001", 4);
        result.Should().Be("0.0001");
    }

    #endregion
}
