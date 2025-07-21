using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalizationManager
{
    public string CurrentLanguageCode { get; private set; } = "en-US";

    // Format: section -> (key -> value)
    private Dictionary<string, Dictionary<string, string>> localizedSections = new();

    private string GetLanguagePath(string langCode) =>
        Path.Combine(Application.streamingAssetsPath, $"Localization/{langCode}.json");

    public void LoadLanguage(string langCode)
    {
        string path = GetLanguagePath(langCode);
        if (!File.Exists(path))
        {
            Debug.LogError($"[Localization] File not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        try
        {
            localizedSections = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            CurrentLanguageCode = langCode;

            //Debug.Log(json);
            //Debug.Log(path);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"[Localization] JSON parse error: {ex.Message}");
        }
    }

    public string Translate(string section, string key)
    {
        if (localizedSections.TryGetValue(section, out var sectionDict) &&
            sectionDict.TryGetValue(key, out var value))
        {
            return value;
        }

        return $"[?{section}.{key}]";
    }
}
