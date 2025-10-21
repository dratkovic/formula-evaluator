using AwesomeAssertions;
using Core;
using Core.Exceptions;
using Xunit;

namespace Tests;

public class StringParserTests
{
    #region ExtractKeyword Tests

    [Fact]
    public void ExtractKeyword_SimpleFunction_ReturnsKeyword()
    {
        var keyword = StringParser.ExtractKeyword("add(2, 3)");
        keyword.Should().Be("add");
    }

    [Fact]
    public void ExtractKeyword_WithWhitespace_ReturnsTrimmedKeyword()
    {
        var keyword = StringParser.ExtractKeyword("  multiply  (2, 3)");
        keyword.Should().Be("multiply");
    }

    [Fact]
    public void ExtractKeyword_MissingParenthesis_ThrowsException()
    {
        var act = () => StringParser.ExtractKeyword("add");
        act.Should().Throw<FormulaParseException>();
    }

    [Fact]
    public void ExtractKeyword_EmptyKeyword_ThrowsException()
    {
        var act = () => StringParser.ExtractKeyword("(2, 3)");
        act.Should().Throw<FormulaParseException>();
    }

    #endregion

    #region ExtractParametersContent Tests

    [Fact]
    public void ExtractParametersContent_SimpleParameters_ReturnsContent()
    {
        var content = StringParser.ExtractParametersContent("add(2, 3)");
        content.Should().Be("2, 3");
    }

    [Fact]
    public void ExtractParametersContent_NestedFunction_ReturnsContent()
    {
        var content = StringParser.ExtractParametersContent("add(2, multiply(3, 4))");
        content.Should().Be("2, multiply(3, 4)");
    }

    [Fact]
    public void ExtractParametersContent_WithWhitespace_ReturnsTrimmedContent()
    {
        var content = StringParser.ExtractParametersContent("add(  2, 3  )");
        content.Should().Be("2, 3");
    }

    [Fact]
    public void ExtractParametersContent_MissingOpenParen_ThrowsException()
    {
        var act = () => StringParser.ExtractParametersContent("add 2, 3)");
        act.Should().Throw<FormulaParseException>();
    }

    [Fact]
    public void ExtractParametersContent_MissingCloseParen_ThrowsException()
    {
        var act = () => StringParser.ExtractParametersContent("add(2, 3");
        act.Should().Throw<FormulaParseException>();
    }

    [Fact]
    public void ExtractParametersContent_MismatchedParens_ThrowsException()
    {
        var act = () => StringParser.ExtractParametersContent(")2, 3(");
        act.Should().Throw<FormulaParseException>();
    }

    #endregion

    #region SplitParameters Tests

    [Fact]
    public void SplitParameters_TwoSimpleParameters_ReturnsList()
    {
        var parameters = StringParser.SplitParameters("2, 3");
        parameters.Count.Should().Be(2);
        parameters[0].Should().Be("2");
        parameters[1].Should().Be("3");
    }

    [Fact]
    public void SplitParameters_WithNestedFunction_ReturnsList()
    {
        var parameters = StringParser.SplitParameters("2, multiply(3, 4)");
        parameters.Count.Should().Be(2);
        parameters[0].Should().Be("2");
        parameters[1].Should().Be("multiply(3, 4)");
    }

    [Fact]
    public void SplitParameters_BothNested_ReturnsList()
    {
        var parameters = StringParser.SplitParameters("add(1, 2), multiply(3, 4)");
        parameters.Count.Should().Be(2);
        parameters[0].Should().Be("add(1, 2)");
        parameters[1].Should().Be("multiply(3, 4)");
    }

    [Fact]
    public void SplitParameters_DeeplyNested_ReturnsList()
    {
        var parameters = StringParser.SplitParameters("2, multiply(add(1, 2), 3)");
        parameters.Count.Should().Be(2);
        parameters[0].Should().Be("2");
        parameters[1].Should().Be("multiply(add(1, 2), 3)");
    }

    [Fact]
    public void SplitParameters_WithWhitespace_ReturnsTrimmedParameters()
    {
        var parameters = StringParser.SplitParameters("  2  ,  3  ");
        parameters.Count.Should().Be(2);
        parameters[0].Should().Be("2");
        parameters[1].Should().Be("3");
    }

    [Fact]
    public void SplitParameters_SingleParameter_ReturnsSingleItem()
    {
        var parameters = StringParser.SplitParameters("42");
        parameters.Count.Should().Be(1);
        parameters[0].Should().Be("42");
    }

    [Fact]
    public void SplitParameters_ThreeParameters_ReturnsThreeItems()
    {
        var parameters = StringParser.SplitParameters("1, 2, 3");
        parameters.Count.Should().Be(3);
        parameters[0].Should().Be("1");
        parameters[1].Should().Be("2");
        parameters[2].Should().Be("3");
    }

    [Fact]
    public void SplitParameters_UnclosedParenthesis_ThrowsException()
    {
        var act = () => StringParser.SplitParameters("2, multiply(3, 4");
        act.Should().Throw<FormulaParseException>();
    }

    [Fact]
    public void SplitParameters_TooManyClosingParens_ThrowsException()
    {
        var act = () => StringParser.SplitParameters("2, multiply(3, 4))");
        act.Should().Throw<FormulaParseException>();
    }

    #endregion

    #region ValidateParameterCount Tests

    [Fact]
    public void ValidateParameterCount_CorrectCount_DoesNotThrow()
    {
        var act = () => StringParser.ValidateParameterCount("add", 2, 2);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateParameterCount_TooFew_ThrowsException()
    {
        var act = () => StringParser.ValidateParameterCount("add", 2, 1);
        act.Should().Throw<InvalidParameterCountException>()
            .Which.Message.Should().Contain("add").And.Contain("2").And.Contain("1");
    }

    [Fact]
    public void ValidateParameterCount_TooMany_ThrowsException()
    {
        var act = () => StringParser.ValidateParameterCount("add", 2, 3);
        act.Should().Throw<InvalidParameterCountException>()
            .Which.Message.Should().Contain("add").And.Contain("2").And.Contain("3");
    }

    #endregion
}
