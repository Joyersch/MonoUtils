namespace MonoUtils.Logic;

public class CalculatorCollection : ICalculator
{
    private List<ICalculator> calculators = [];

    public void Register(ICalculator calculator)
        => calculators.Add(calculator);

    public void Apply()
    {
        foreach (var calculator in calculators)
            calculator.Apply();
    }
}