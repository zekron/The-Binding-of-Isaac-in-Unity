using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PlayerProfileWindow : ProfileWindow<PlayerProfileTreeElement>
{
    private PlayerProfileTreeView m_TreeView;
    private PlayerProfileTreeAsset m_MyTreeAsset;

    private const string PATH_PLAYERPROFILE_SO = "Assets/ScriptableObjects/";
    private const string FILE_PLAYERPROFILE_SO = "PlayerProfile TreeAsset.asset";

    [MenuItem("Custom Menu/Window/Player Profile Editor")]
    private static void CreateWindow()
    {
        EditorWindow window = GetWindow<PlayerProfileWindow>("Player Profile Editor", true);
        //window.minSize = window.maxSize = new Vector2(600, 600);
    }

    protected override void InitIfNeeded()
    {
        if (!m_Initialized)
        {
            // Check if it already exists (deserialized from window layout file or scriptable object)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            bool firstInit = m_MultiColumnHeaderState == null;
            var headerState = PlayerProfileTreeView.CreateDefaultMultiColumnHeaderState(multiColumnTreeViewRect.width);
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
                MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
            m_MultiColumnHeaderState = headerState;

            var multiColumnHeader = new MultiColumnHeader(headerState);
            if (firstInit)
                multiColumnHeader.ResizeToFit();
            multiColumnHeader.height = 20;
            var treeModel = new TreeModel<PlayerProfileTreeElement>(GetData());

            m_TreeView = new PlayerProfileTreeView(m_TreeViewState, multiColumnHeader, treeModel);

            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;

            m_Initialized = true;
        }

        SetSorting();
    }
    protected override void SearchBar(Rect rect)
    {
        m_TreeView.searchString = m_SearchField.OnGUI(rect, m_TreeView.searchString);
    }

    protected override void DoTreeView(Rect rect)
    {
        m_TreeView.OnGUI(rect);
    }

    protected override void BottomToolBar(Rect rect)
    {
        GUILayout.BeginArea(rect);

        using (new EditorGUILayout.HorizontalScope())
        {

            var style = "miniButton";

            GUILayout.Label(m_MyTreeAsset != null ? AssetDatabase.GetAssetPath(m_MyTreeAsset) : string.Empty);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Set sorting", style))
            {
                SetSorting();
            }

            GUILayout.FlexibleSpace();

            SetEditModeToggle(m_TreeView.InEditMode);

            GUILayout.Space(20);

            if (GUILayout.Button("Add Player Profile", style))
            {
                AddPlayerProfile();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Delete Player Profile", style))
            {
                DeletePlayerProfile();
            }
        }

        GUILayout.EndArea();
    }

    protected override void SetEditModeToggle(bool value)
    {
        m_TreeView.InEditMode = !GUILayout.Toggle(value, "Edit Mode");
    }

    protected override IList<PlayerProfileTreeElement> GetData()
    {
        if (m_MyTreeAsset != null && m_MyTreeAsset.TreeElements != null && m_MyTreeAsset.TreeElements.Count > 0)
            return m_MyTreeAsset.TreeElements;
        else
        {
            CheckFileExists(PATH_PLAYERPROFILE_SO, FILE_PLAYERPROFILE_SO, typeof(PlayerProfileTreeAsset));

            m_MyTreeAsset = AssetDatabase.LoadAssetAtPath<PlayerProfileTreeAsset>(Path.Combine(PATH_PLAYERPROFILE_SO, FILE_PLAYERPROFILE_SO));

            EditorUtility.SetDirty(m_MyTreeAsset);
            return m_MyTreeAsset.TreeElements;
        }
    }

    protected override void AddPlayerProfile()
    {
        if (CheckTreeAsset())
        {
            m_TreeView.CustomTreeModel.AddElement(m_MyTreeAsset.CreateProfile(), m_MyTreeAsset.TreeRoot, Mathf.Max(0, m_MyTreeAsset.TreeRoot.Children.Count - 1));
            DoTreeView(multiColumnTreeViewRect);
        }
        else Debug.Log("AddPlayerProfile");

        SetEditModeToggle(true);

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override void DeletePlayerProfile()
    {
        if (CheckTreeAsset())
        {
            IList<int> selections = m_TreeView.GetSelection();
            m_TreeView.CustomTreeModel.RemoveElements(selections);
        }
        else Debug.Log("DeletePlayerProfile");

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override bool CheckTreeAsset()
    {
        return m_MyTreeAsset != null && m_MyTreeAsset.TreeElements != null && m_MyTreeAsset.TreeElements.Count > 0;
    }

    protected override void SetSorting()
    {
        var myColumnHeader = (MultiColumnHeader)m_TreeView.multiColumnHeader;
        myColumnHeader.SetSortingColumns(new int[] { 0, 1 }, new[] { true, true });
    }
}
