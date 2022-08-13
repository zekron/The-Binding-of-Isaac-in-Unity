using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class ItemEditorWindow : EditorWindow
{
    private TreeViewState treeViewState;
    protected MultiColumnHeaderState m_MultiColumnHeaderState;
    protected SearchField m_SearchField;

    private bool initialized;

    protected void OnGUI()
    {
        InitIfNeeded();
    }

    protected virtual void InitIfNeeded()
    {
        if (!initialized)
        {
            // Check if it already exists (deserialized from window layout file or scriptable object)
            if (treeViewState == null)
                treeViewState = new TreeViewState();


        }
    }
    protected static void CheckFileExists(string path, string fileName, Type type)
    {
        string assetPath = Path.Combine(path, fileName);
        if (!File.Exists(assetPath))
        {
            var data = CreateInstance(type);

            AssetDatabase.CreateAsset(data, assetPath);
            Debug.LogWarning(string.Format("File not found. Create {0} success.", fileName));
        }
    }
    protected abstract MultiColumnHeaderState GetHeaderState();
    protected abstract IList<CollectableItemTreeElement> GetData();
    protected abstract void AddPlayerProfile();
    protected abstract void DeletePlayerProfile();
}
