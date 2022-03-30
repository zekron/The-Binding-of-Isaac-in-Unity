using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class ProfileWindow<T> : EditorWindow where T :CharacterProfileTreeElement
{
    protected TreeViewState m_TreeViewState;
    protected MultiColumnHeaderState m_MultiColumnHeaderState;
    protected SearchField m_SearchField;
    //protected EnemyProfileTreeView m_TreeView;
    //protected CharacterProfileTreeAsset m_MyTreeAsset;

    protected bool m_Initialized;

    #region Rect
    protected Rect multiColumnTreeViewRect
    {
        get { return new Rect(20, 30, position.width - 40, position.height - 60); }
    }

    protected Rect toolbarRect
    {
        get { return new Rect(20f, 10f, position.width - 40f, 20f); }
    }

    protected Rect bottomToolbarRect
    {
        get { return new Rect(20f, position.height - 18f, position.width - 40f, 16f); }
    }
    #endregion

    protected void OnGUI()
    {
        InitIfNeeded();

        SearchBar(toolbarRect);
        DoTreeView(multiColumnTreeViewRect);
        BottomToolBar(bottomToolbarRect);
    }

    protected abstract void InitIfNeeded();
    //{
    //    if (!m_Initialized)
    //    {
    //        // Check if it already exists (deserialized from window layout file or scriptable object)
    //        if (m_TreeViewState == null)
    //            m_TreeViewState = new TreeViewState();

    //        bool firstInit = m_MultiColumnHeaderState == null;
    //        var headerState = EnemyProfileTreeView.CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
    //        if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
    //            MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
    //        m_MultiColumnHeaderState = headerState;

    //        var multiColumnHeader = new MultiColumnHeader(headerState);
    //        if (firstInit)
    //            multiColumnHeader.ResizeToFit();
    //        multiColumnHeader.height = 20;
    //        var treeModel = new TreeModel<CharacterProfileTreeElement>(GetData());

    //        m_TreeView = new EnemyProfileTreeView(m_TreeViewState, multiColumnHeader, treeModel);

    //        m_SearchField = new SearchField();
    //        m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;

    //        m_Initialized = true;
    //    }

    //    SetSorting();
    //}

    protected abstract void SearchBar(Rect rect);
    //{
    //    m_TreeView.searchString = m_SearchField.OnGUI(rect, m_TreeView.searchString);
    //}

    protected abstract void DoTreeView(Rect rect);
    //{
    //    m_TreeView.OnGUI(rect);
    //}

    protected abstract void BottomToolBar(Rect rect);
    //{
    //    GUILayout.BeginArea(rect);

    //    using (new EditorGUILayout.HorizontalScope())
    //    {

    //        var style = "miniButton";

    //        GUILayout.Label(m_MyTreeAsset != null ? AssetDatabase.GetAssetPath(m_MyTreeAsset) : string.Empty);

    //        GUILayout.FlexibleSpace();

    //        if (GUILayout.Button("Set sorting", style))
    //        {
    //            SetSorting();
    //        }

    //        GUILayout.FlexibleSpace();

    //        SetEditModeToggle(m_TreeView.InEditMode);

    //        GUILayout.Space(20);

    //        if (GUILayout.Button("Add Player Profile", style))
    //        {
    //            AddPlayerProfile();
    //        }

    //        GUILayout.Space(20);

    //        if (GUILayout.Button("Delete Player Profile", style))
    //        {
    //            DeletePlayerProfile();
    //        }
    //    }

    //    GUILayout.EndArea();
    //}

    protected abstract void SetEditModeToggle(bool value);
    //{
    //    m_TreeView.InEditMode = !GUILayout.Toggle(value, "Edit Mode");
    //}

    protected abstract IList<T> GetData() ;
    //{
    //    if (CheckTreeAsset())
    //        return m_MyTreeAsset.TreeElements;
    //    else
    //        return CharacterProfileTreeAsset.GetTestData();
    //}

    protected abstract void AddPlayerProfile();
    //{
    //    if (CheckTreeAsset())
    //    {
    //        m_TreeView.CustomTreeModel.AddElement(m_MyTreeAsset.CreateProfile(), m_MyTreeAsset.TreeRoot, Mathf.Max(0, m_MyTreeAsset.TreeRoot.Children.Count - 1));
    //        DoTreeView(multiColumnTreeViewRect);
    //    }
    //    else Debug.Log("AddPlayerProfile");

    //    SetEditModeToggle(true);

    //    EditorUtility.SetDirty(m_MyTreeAsset);
    //}

    protected abstract void DeletePlayerProfile();
    //{
    //    if (CheckTreeAsset())
    //    {
    //        IList<int> selections = m_TreeView.GetSelection();
    //        m_TreeView.CustomTreeModel.RemoveElements(selections);
    //    }
    //    else Debug.Log("DeletePlayerProfile");

    //    EditorUtility.SetDirty(m_MyTreeAsset);
    //}

    protected abstract bool CheckTreeAsset();
    //{
    //    return m_MyTreeAsset != null && m_MyTreeAsset.TreeElements != null && m_MyTreeAsset.TreeElements.Count > 0;
    //}

    protected abstract void SetSorting();
    //{
    //    var myColumnHeader = (MultiColumnHeader)m_TreeView.multiColumnHeader;
    //    myColumnHeader.SetSortingColumns(new int[] { 0, 1 }, new[] { true, true });
    //}

    protected static void CheckFileExists(string path, string fileName, Type type)
    {
        string assetPath = Path.Combine(path, fileName);
        if (!File.Exists(assetPath))
        {
            var data = ScriptableObject.CreateInstance(type);

            AssetDatabase.CreateAsset(data, assetPath);
            Debug.LogWarning(string.Format("File not found. Create {0} success.", fileName));
        }
    }
}
