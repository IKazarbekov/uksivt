namespace Calculator;

public class Logic
{
    public double Calculate(double a, double b, string op) => op switch
    {
        "+" => a + b,
        "-" => a - b,
        "*" => a * b,
        "/" => b == 0 ? 0 : a / b,
        _ => b
    };
}