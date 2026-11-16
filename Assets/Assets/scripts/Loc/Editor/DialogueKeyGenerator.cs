#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Globalization;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;

public static class DialogueKeyGenerator
{
    [MenuItem("Tools/Localization/Sync Dialogue Graphs to String Table")]
    static void Sync()
    {
        var collection = LocalizationEditorSettings.GetStringTableCollection(DialogueLoc.DTable);
        if (collection == null)
        {
            Debug.LogError("String Table Collection not found");
            return;
        }

        var en = collection.GetTable("en") as StringTable;
        if (en == null)
        {
            Debug.LogError("No 'en' table in the Dialogue collection.");
            return;
        }

        foreach (string guid in AssetDatabase.FindAssets("t:DialogueGraph"))
        {
            var graph = AssetDatabase.LoadAssetAtPath<DialogueGraph>(AssetDatabase.GUIDToAssetPath(guid));
            int num = 1;

            foreach (var node in graph.nodes)
            {
                var d = node as DialogueNode;
                if (d == null) continue;

                if (string.IsNullOrEmpty(d.lineKey))
                {
                    string key;
                    do
                    {
                        key = graph.name + "_" + num.ToString("000");
                        num++;
                    }
                    while (en.GetEntry(key) != null);
                    d.lineKey = key;
                }
                Put(en, d.lineKey, d.dialogLine);
                if (!string.IsNullOrEmpty(d.passengerName))
                    Put(en, "Name_" + d.passengerName, d.passengerName);
                EditorUtility.SetDirty(d);

            }
                EditorUtility.SetDirty(graph);
        }

            EditorUtility.SetDirty(en);
            EditorUtility.SetDirty(collection.SharedData);
            AssetDatabase.SaveAssets();
            Debug.Log("sync done");
     }

        static void Put(StringTable table, string key, string value)
        {
            var entry = table.GetEntry(key);
            if (entry == null) table.AddEntry(key, value);
            else entry.Value = value;
        }

}
#endif
