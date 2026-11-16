using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text.RegularExpressions;

public static class DialogueLoc 
{
    public const string DTable = "DialogueLoc"; //String table collection  
    public const string UITable = "UI"; //String table collection

    public static string Text(string key, string table)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
    }

    public static string GetUItext(string key)
    {
        return Text(key, UITable);
    }

    public static AsyncOperationHandle<string> GetD(string key)
    {
        return Get(key, DTable);
    }

    public static AsyncOperationHandle<string> GetUI(string key)
    {
        return Get(key, UITable);
    }

    public static AsyncOperationHandle<string> Get(string key, string table)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);
    }
}
