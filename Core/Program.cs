using Core;

var evaluator = new FormulaEvaluator();

Console.WriteLine("=== Formula Evaluator Demo ===\n");

// Simple examples
Console.WriteLine("Simple numbers:");
Console.WriteLine($"  42 = {evaluator.Evaluate("42")}");
Console.WriteLine($"  -5 = {evaluator.Evaluate("-5")}");
Console.WriteLine($"  3.14 = {evaluator.Evaluate("3.14")}");

Console.WriteLine("\nBasic operations:");
Console.WriteLine($"  add(2, 3) = {evaluator.Evaluate("add(2, 3)")}");
Console.WriteLine($"  subtract(10, 4) = {evaluator.Evaluate("subtract(10, 4)")}");
Console.WriteLine($"  multiply(3, 4) = {evaluator.Evaluate("multiply(3, 4)")}");
Console.WriteLine($"  divide(8, 2) = {evaluator.Evaluate("divide(8, 2)")}");

Console.WriteLine("\nNested expressions:");
Console.WriteLine($"  add(2, multiply(3, 4)) = {evaluator.Evaluate("add(2, multiply(3, 4))")}");
Console.WriteLine($"  multiply(add(2, 3), divide(10, 2)) = {evaluator.Evaluate("multiply(add(2, 3), divide(10, 2))")}");
Console.WriteLine($"  add(1, add(2, add(3, 4))) = {evaluator.Evaluate("add(1, add(2, add(3, 4)))")}");

Console.WriteLine("\nComplex nested:");
Console.WriteLine($"  add(2, multiply(add(1, multiply(1, 1)), divide(2, 2))) = {evaluator.Evaluate("add(2, multiply(add(1, multiply(1, 1)), divide(2, 2)))")}");

Console.WriteLine("\nDecimal precision:");
Console.WriteLine($"  divide(10, 3) [2 places] = {evaluator.Evaluate("divide(10, 3)")}");
Console.WriteLine($"  divide(10, 3) [4 places] = {evaluator.Evaluate("divide(10, 3)", 4)}");

Console.WriteLine("\nError handling:");
Console.WriteLine($"  divide(10, 0) = {evaluator.Evaluate("divide(10, 0)")}");
Console.WriteLine($"  unknown(2, 3) = {evaluator.Evaluate("unknown(2, 3)")}");
Console.WriteLine($"  add(2) = {evaluator.Evaluate("add(2)")}");
Console.WriteLine($"  That's all folks = {evaluator.Evaluate(" That's all folks")}");

