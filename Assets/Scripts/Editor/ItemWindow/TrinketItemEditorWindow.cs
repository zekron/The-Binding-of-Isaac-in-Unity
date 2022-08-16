using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TrinketItemEditorWindow : ItemEditorWindow<ItemTreeElement>
{
    protected override void AddPlayerProfile()
    {
        if (CheckTreeAsset())
        {
            m_TreeView.CustomTreeModel.AddElement(m_MyTreeAsset.CreateProfile(),
                                                  m_MyTreeAsset.TreeRoot,
                                                  Mathf.Max(0, m_MyTreeAsset.TreeRoot.Children.Count - 1));
            DoTreeView(MultiColumnTreeViewRect);
        }
        else Debug.Log("AddPlayerProfile");

        SetEditModeToggle(true);

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override ItemTreeView<ItemTreeElement> CreateTreeView(MultiColumnHeader multiColumnHeader, TreeModel<ItemTreeElement> treeModel)
    {
        return new TrinketItemTreeView(treeViewState, multiColumnHeader, treeModel);
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

    protected override IList<ItemTreeElement> GetData()
    {
        if (CheckTreeAsset())
            return m_MyTreeAsset.TreeElements;
        else
        {
            CheckFileExists(StaticData.ScriptableObjectFolderPath,
                            StaticData.FILE_TRINKETITEM_SO,
                            typeof(TrinketItemTreeAsset));

            m_MyTreeAsset = AssetDatabase.LoadAssetAtPath<TrinketItemTreeAsset>(Path.Combine(StaticData.ScriptableObjectFolderPath,
                                                                                               StaticData.FILE_TRINKETITEM_SO));

            EditorUtility.SetDirty(m_MyTreeAsset);
            return m_MyTreeAsset.TreeElements;
        }
    }

    protected override MultiColumnHeaderState GetHeaderState()
    {
        var columns = new MultiColumnHeaderState.Column[ItemTreeElement.TreeViewColumnsLength];

        columns[(int)ItemColumns.ID] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Character ID", "ID of character"),
            contextMenuText = "Type",
            headerTextAlignment = TextAlignment.Left,
            sortedAscending = true,
            sortingArrowAlignment = TextAlignment.Right,
            width = 100,
            minWidth = 100,
            maxWidth = 200,
            autoResize = false,
            allowToggleVisibility = true
        };
        columns[(int)ItemColumns.ItemSprite] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Trinket Sprite"),
            contextMenuText = "Type",
            headerTextAlignment = TextAlignment.Right,
            sortedAscending = true,
            sortingArrowAlignment = TextAlignment.Right,
            width = 100,
            minWidth = 100,
            maxWidth = 200,
            autoResize = false,
            allowToggleVisibility = true
        };
        columns[(int)ItemColumns.Name] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Trinket Name", "Name of Trinket"),
            contextMenuText = "Type",
            headerTextAlignment = TextAlignment.Left,
            sortedAscending = true,
            sortingArrowAlignment = TextAlignment.Right,
            width = 100,
            minWidth = 100,
            maxWidth = 200,
            autoResize = false,
            allowToggleVisibility = true
        };
        columns[(int)ItemColumns.Description] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Description"),
            contextMenuText = "Type",
            headerTextAlignment = TextAlignment.Left,
            sortedAscending = true,
            sortingArrowAlignment = TextAlignment.Right,
            width = 300,
            minWidth = 200,
            maxWidth = 400,
            autoResize = false,
            allowToggleVisibility = true
        };


        var state = new MultiColumnHeaderState(columns);
        return state;
    }
}
