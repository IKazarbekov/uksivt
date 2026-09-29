using Xunit;
using Calculator;
using System.Reflection.Metadata;
namespace MyApp.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(2, 3, "+", 5)]
    [InlineData(2.50, 32, "+", 34.50)]
    [InlineData(2, 3, "-", -1)]
    [InlineData(20, -3.43, "-", 23.43)]
    [InlineData(2, 3, "*", 6)]
    [InlineData(5, 3.5, "*", 17.5)]
    [InlineData(9, 3, "/", 3)]
    [InlineData(17.5, 5, "/", 3.5)]
    public void Test1(double a, double b, string operation, double result)
    {
        var logic = new Logic();
        double result1 = logic.Calculate(a, b, operation);
        Assert.True(result1 == result);
    }

    [Fact]
    public void Test2()
    {
        var logic = new Logic();
        double result1 = logic.Calculate(5, 0, "/");
        Assert.True(result1 == 0);
    }
}