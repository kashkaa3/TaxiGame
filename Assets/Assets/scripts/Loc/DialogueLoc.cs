using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text.RegularExpressions;

public static class DialogueLoc 
{
    public const string Table = "DialogueLoc"; //String table collection  

    public static AsyncOperationHandle<string> Get (string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(Table, key);
    }
}
