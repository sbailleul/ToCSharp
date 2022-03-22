namespace ToCSharp.Core.Extensions;

public static class StringExtensions
{
    public static string UpperFirst(this string txt)
    {
        if (string.IsNullOrEmpty(txt)) return txt;

        var firstLetter = txt[..1].ToUpper();
        if (txt.Length == 1) return firstLetter;

        return string.Concat(firstLetter, txt.AsSpan(1));
    }

    public static string LowerFirst(this string txt)
    {
        if (string.IsNullOrEmpty(txt)) return txt;

        var firstLetter = txt[..1].ToLower();
        if (txt.Length == 1) return firstLetter;

        return string.Concat(firstLetter, txt.AsSpan(1));
    }
}