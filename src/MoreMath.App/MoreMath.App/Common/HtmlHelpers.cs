using Microsoft.AspNetCore.Components;

namespace MoreMath.App.Common;

public static class HtmlHelpers
{
    public static MarkupString ToMarkupWithProcessing(this string text)
    {
        text = text.Replace(" --", "&nbsp;&ndash;");
        text = text.Replace(" ---", "&nbsp;&mdash;");
        text = text.Replace("\\\\", "</p><p>");
        text = text.Replace("\\-", "&shy;");
        return new MarkupString(text);
    }
}
