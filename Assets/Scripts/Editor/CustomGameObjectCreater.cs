using UnityEditor;
using UnityEngine;

public class CustomGameObjectCreater
{
    [MenuItem("GameObject/UI/Custom/MiniMap")]
    private static void CreateMiniMap(MenuCommand menuCommand)
    {
        CreateGameObject<MiniMap>("Mini Map", menuCommand);
    }

    #region Utilities
    private static void CreateGameObject<T>(string name, MenuCommand menuCommand)
        => CreateGameObject(typeof(T), name, menuCommand);
    private static void CreateGameObject(System.Type T, string name, MenuCommand menuCommand)
    {
        GameObject go = new GameObject(name);
        go.AddComponent(T);

        GameObjectUtility.SetParentAndAlign(go, (GameObject)menuCommand.context);

        Undo.RegisterCreatedObjectUndo(go, string.Format("Create {0}", go.name));

        Selection.activeObject = go;
    }
    #endregion
}
