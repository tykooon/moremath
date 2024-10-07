using Microsoft.AspNetCore.Components;

namespace MoreMath.App.Extensions;

public static class HtmlExtesions
{
    public static MarkupString ToHtml(this string text)
    {
        return new MarkupString(text.Replace("~~", "&nbsp;"));
    }
}
