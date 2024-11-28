namespace MoreMath.App.Common;

public class CardsTotal
{
    private CardsTotal(int amount, string optionTitle)
    {
        Amount = amount;
        OptionName = optionTitle;
    }

    public int Amount { get; set; } = 10;
    public string OptionName { get; set; } = "";

    public readonly static List<CardsTotal> Options = 
    [
        new(10, "10 шт."),
        new(20, "20 шт."),
        new(30, "30 шт."),
        new(40, "40 шт."),
        new(50, "50 шт.")
    ];
}


