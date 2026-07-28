using MonoMod;
using UnityEngine;
using System.Collections.Generic;

[MonoModPatch("global::Language.Language")]
public static class Language
{
    [MonoModIgnore]
    private static Dictionary<string, Dictionary<string, string>> currentEntrySheets;

    public static string Get(string key, string sheetTitle)
    {
        if (currentEntrySheets == null || !currentEntrySheets.ContainsKey(sheetTitle))
        {
            Debug.LogError($"The sheet with title \"{sheetTitle}\" does not exist!");
            return string.Empty;
        }

        if (currentEntrySheets[sheetTitle].ContainsKey(key))
        {
            string textValue = currentEntrySheets[sheetTitle][key];

            if (Patches.GameManagerPatch.UndeadnameJasmine && textValue == "Jack Vine") textValue = "Jasmine Vine";

            return textValue;
        }

        return "#!#" + key + "#!#";
    }
}
