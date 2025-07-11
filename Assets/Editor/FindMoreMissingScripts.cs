using UnityEngine;
using UnityEditor;
public class FindMoreMissingScripts : EditorWindow
{

    [MenuItem("Window/FindMoreMissingScripts")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindMoreMissingScripts));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
        {
            FindInSelected();
        }
    }
    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        int go_count = 0, components_count = 0, missing_count = 0;
        foreach (GameObject g in go)
        {
            go_count++;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                components_count++;
                if (components[i] == null)
                {
                    missing_count++;
                    Debug.Log(g.name + " has an empty script attached in position: " + i, g);
                }
            }
        }

        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }
}