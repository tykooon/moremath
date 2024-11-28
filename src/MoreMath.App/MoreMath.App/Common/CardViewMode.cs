namespace MoreMath.App.Common;

public class CardViewMode
{
    private CardViewMode(string id, string mode)
    {
        ID = id;
        ViewMode = mode;
    }

    public string ID { get; set; } = "";
    public string ViewMode { get; set; } = "";

    public readonly static List<CardViewMode> Options = 
    [
        new("Regular", "Обычный"),
        new("HebrewFirst", "Сначала иврит"),
        new("RussianFirst","Сначала перевод"),
    ];


}


