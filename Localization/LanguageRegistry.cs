using System.Collections.Generic;

public static class LanguageRegistry
{
    public static readonly Dictionary<string, string> LanguageNames = new()
    {
        { "en-US", "English (United States)" },
        { "en-GB", "English (United Kingdom)" },
        { "es-ES", "Español (España)" },
        { "es-MX", "Español (México)" },
        { "fr-FR", "Français (France)" },
        { "de-DE", "Deutsch (Deutschland)" },
        { "it-IT", "Italiano (Italia)" },
        { "pt-BR", "Português (Brasil)" },
        { "pt-PT", "Português (Portugal)" },
        { "pl-PL", "Polski (Polska)" },
        { "tr-TR", "Türkçe (Türkiye)" },
        { "nl-NL", "Nederlands (Nederland)" },
        { "sv-SE", "Svenska (Sverige)" },
        { "fi-FI", "Suomi (Suomi)" },
        { "ru-RU", "Русский (Россия)" },
        /*{ "ar-SA", "العربية (السعودية)" },
        { "ja-JP", "日本語 (日本)" },
        { "ko-KR", "한국어 (대한민국)" },
        { "zh-CN", "中文 (简体, 中国)" },
        { "zh-TW", "中文 (繁體, 台灣)" },*/
    };

    public static string GetDisplayNameByIndex(int index)
    {
        if (index < 0 || index >= LanguageNames.Count)
            return null;

        int i = 0;
        foreach (var pair in LanguageNames)
        {
            if (i == index)
                return pair.Value;
            i++;
        }
        return null;
    }

    public static int GetIndexByCode(string code)
    {
        int index = 0;
        foreach (var key in LanguageNames.Keys)
        {
            if (key == code)
                return index;
            index++;
        }
        return -1;
    }

    public static string GetCodeByIndex(int index)
    {
        if (index < 0 || index >= LanguageNames.Count)
            return null;

        int i = 0;
        foreach (var key in LanguageNames.Keys)
        {
            if (i == index)
                return key;
            i++;
        }
        return null;
    }

    public static string GetDisplayName(string code) =>
        LanguageNames.TryGetValue(code, out var name) ? name : $"Unknown ({code})";

    public static IEnumerable<string> GetAllCodes() => LanguageNames.Keys;

    public static IEnumerable<string> GetAllNames() => LanguageNames.Values;
}