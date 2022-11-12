using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CollectibleItemEditorWindow : ItemEditorWindow<CollectibleItemTreeElement>
{
    List<int> selectedID = new List<int>(1);
    protected override void AddPlayerProfile()
    {
        if (CheckTreeAsset())
        {
            var element = m_MyTreeAsset.CreateProfile();
            m_TreeView.CustomTreeModel.AddElement(element,
                                                  m_MyTreeAsset.TreeRoot,
                                                  Mathf.Max(0, m_MyTreeAsset.TreeRoot.Children.Count - 1));
            DoTreeView(MultiColumnTreeViewRect);

            if (selectedID.Count == 1)
                selectedID[0] = element.ElementID;
            else
                selectedID.Add(element.ElementID);
            m_TreeView.SetFocus();
            m_TreeView.SetSelection(selectedID, TreeViewSelectionOptions.RevealAndFrame);
        }
        else Debug.Log("AddPlayerProfile");

        SetEditModeToggle(true);

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override ItemTreeView<CollectibleItemTreeElement> CreateTreeView(MultiColumnHeader multiColumnHeader, TreeModel<CollectibleItemTreeElement> treeModel)
    {
        return new CollectibleItemTreeView(treeViewState, multiColumnHeader, treeModel);
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

    protected override IList<CollectibleItemTreeElement> GetData()
    {
        if (CheckTreeAsset())
            return m_MyTreeAsset.TreeElements;
        else
        {
            CheckFileExists(StaticData.ScriptableObjectFolderPath,
                            StaticData.FILE_COLLECTIBLEITEM_SO,
                            typeof(CollectibleItemTreeAsset));

            m_MyTreeAsset = AssetDatabase.LoadAssetAtPath<CollectibleItemTreeAsset>(Path.Combine(StaticData.ScriptableObjectFolderPath,
                                                                                               StaticData.FILE_COLLECTIBLEITEM_SO));

            EditorUtility.SetDirty(m_MyTreeAsset);
            return m_MyTreeAsset.TreeElements;
        }
    }

    protected override MultiColumnHeaderState GetHeaderState()
    {
        var columns = new MultiColumnHeaderState.Column[CollectibleItemTreeElement.TreeViewColumnsLength];

        columns[(int)ItemColumns.ID] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Item ID", "ID of item"),
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
            headerContent = new GUIContent("Item Sprite"),
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
        columns[(int)ItemColumns.CollectionSprite] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Collection Sprite"),
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
        columns[(int)ItemColumns.Quote] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Quote"),
            contextMenuText = "Type",
            headerTextAlignment = TextAlignment.Left,
            sortedAscending = true,
            sortingArrowAlignment = TextAlignment.Right,
            width = 200,
            minWidth = 200,
            maxWidth = 400,
            autoResize = false,
            allowToggleVisibility = true
        };
        columns[(int)ItemColumns.ItemPrefab] = new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent("Item Prefab"),
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


        var state = new MultiColumnHeaderState(columns);
        return state;
    }
}
