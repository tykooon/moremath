namespace MoreMath.Shared.Extensions;

public static class HebrewExtensions
{
    internal static HashSet<char> HebrewAlphabet =
        ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט', 'י', 'כ', 'ך', 'ל', 'מ', 'ם',
            'נ', 'ן', 'ס', 'ע', 'פ', 'ף', 'צ', 'ץ', 'ק', 'ר', 'ש', 'ת', '\'', '־'];

    internal static HashSet<char> HebrewNiqqud =
    ['ְ', 'ֱ', 'ֲ', 'ֳ', 'ִ', 'ֵ', 'ֶ', 'ַ', 'ָ', 'ׂ', 'ׁ', 'ֹ', 'ּ', 'ֻ'];

    public static string RemoveNonHebrew(this string word, bool niqqudAllowed = true)
    {
        char[] buffer = new char[word.Length];
        int index = 0;
        foreach (char c in word)
        {
            if (HebrewAlphabet.Contains(c) || (niqqudAllowed && HebrewNiqqud.Contains(c)))
            {
                buffer[index] = c;
                index++;
            }
        }
        return new string(buffer, 0, index);
    }

    public static string RemoveNiqqud(this string word) => RemoveNonHebrew(word, false);
}
