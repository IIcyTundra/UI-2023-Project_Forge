
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item), true)]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Item itemData = (Item)target;

        // Display the default inspector
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Stats"))
        {
            itemData.GenerateStats();
            EditorUtility.SetDirty(target);
        }
    }
}
