using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class TrinketItemTreeView : ItemTreeView<ItemTreeElement>
{
    enum SortOption
    {
        ID, Name
    }
    public TrinketItemTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, TreeModel<ItemTreeElement> model) : base(state, multiColumnHeader, model) { }

    protected override void CellGUI(Rect cellRect, TreeViewItem<ItemTreeElement> item, ItemColumns column, ref RowGUIArgs args)
    {
        CenterRectUsingSingleLineHeight(ref cellRect);

        if (InEditMode)
        {
            switch (column)
            {
                case ItemColumns.ID:
                    item.data.ElementID = EditorGUI.IntField(cellRect, item.data.ElementID);
                    break;
                case ItemColumns.Name:
                    item.data.name = GUI.TextField(cellRect, item.data.name);
                    break;
                case ItemColumns.Description:
                    item.data.Description = GUI.TextArea(cellRect, item.data.Description);
                    break;
                case ItemColumns.ItemSprite:
                    item.data.ItemSprite = EditorGUI.ObjectField(cellRect, item.data.ItemSprite, typeof(Sprite), false) as Sprite;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (column)
            {
                case ItemColumns.ID:
                    EditorGUI.LabelField(cellRect, item.data.ElementID.ToString("D3"));
                    break;
                case ItemColumns.Name:
                    EditorGUI.LabelField(cellRect, item.data.name.ToString());
                    break;
                case ItemColumns.Description:
                    EditorGUI.LabelField(cellRect, item.data.Description.ToString());
                    break;
                case ItemColumns.ItemSprite:
                    GUI.DrawTexture(cellRect, item.data.ItemSprite.texture);
                    break;
                default:
                    break;
            }
        }
    }

    protected override IOrderedEnumerable<TreeViewItem<ItemTreeElement>> InitialOrder(IEnumerable<TreeViewItem<ItemTreeElement>> myTypes, int[] history)
    {
        bool ascending = multiColumnHeader.IsSortedAscending(history[0]);

        if ((SortOption)history[0] == SortOption.Name)
            return myTypes.Order(l => l.data.name, ascending);
        else
            return myTypes.Order(l => l.data.ElementID, ascending);
    }

    protected override void SortByMultipleColumns()
    {
        var sortedColumns = multiColumnHeader.state.sortedColumns;

        if (sortedColumns.Length == 0)
            return;

        var myTypes = rootItem.children.Cast<TreeViewItem<ItemTreeElement>>();
        var orderedQuery = InitialOrder(myTypes, sortedColumns);
        for (int i = 1; i < sortedColumns.Length; i++)
        {
            bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);
            if ((SortOption)sortedColumns[i] == SortOption.ID)
                orderedQuery = orderedQuery.ThenBy(l => l.data.ElementID, ascending);
            else if ((SortOption)sortedColumns[i] == SortOption.Name)
                orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
        }

        rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
    }
}
