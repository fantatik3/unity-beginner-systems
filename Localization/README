### LocalizationManager

A minimal, extensible localization system for Unity supporting multiple languages via structured JSON files.

#### Features

- Loads language-specific JSON files from `StreamingAssets/Localization/`.
- Fast key-based lookup with section support: `Translate(section, key)`.
- Uses `Newtonsoft.Json` for modern JSON parsing.
- Supports fallback for missing keys.
- Simple integration with UI and game state.

---

### Setup

1. Place language files (e.g., `en-US.json`, `es-ES.json`) in `Assets/StreamingAssets/Localization/`.
2. Ensure each file matches this structure:

```json
{
  "MainMenu": {
    "title": "Welcome",
    "start": "Start Game"
  },
  "OptionsMenu": {
    "language": "Language",
    "save": "Save"
  }
}
```

3. Call `LocalizationManager.LoadLanguage("en-US")` to initialize.

### Usage

#### Initialization

```csharp
LocalizationManager localizationManager = new LocalizationManager();
localizationManager.LoadLanguage("en-US");
```

#### Lookup

```csharp
string startLabel = localizationManager.Translate("MainMenu", "start");
// Returns: "Start Game"
```

#### Dynamic UI Example

```csharp
titleText.text = localizationManager.Translate("MainMenu", "title");
saveButtonText.text = localizationManager.Translate("OptionsMenu", "save");
```

### Notes

- Missing keys return `"[?Section.Key]"` for debugging.
