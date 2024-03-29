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
        if (column == ItemColumns.ID || column == ItemColumns.Name || column == ItemColumns.Quote || column == ItemColumns.ItemPrefab)
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
                case ItemColumns.Quote:
                    item.data.ItemQuote = GUI.TextField(cellRect, item.data.ItemQuote);
                    break;
                case ItemColumns.Description:
                    var center = cellRect.center;
                    cellRect.height -= 6;
                    cellRect.center = center;
                    item.data.ItemDescription = GUI.TextArea(cellRect, item.data.ItemDescription);
                    break;
                case ItemColumns.ItemSprite:
                    center = cellRect.center;
                    cellRect.height -= 4;
                    cellRect.width = cellRect.height;
                    cellRect.center = center;
                    item.data.ItemSprite = EditorGUI.ObjectField(cellRect,
                                                                 item.data.ItemSprite,
                                                                 typeof(Sprite),
                                                                 false) as Sprite;
                    if (item.data.ItemSprite != null && item.data.name == "Name here")
                    {
                        var strs = item.data.ItemSprite.name.Split('_');
                        if (strs.Length == 2)
                        {
                            item.data.ElementID = int.Parse(strs[0]);
                            item.data.name = strs[1];
                        }
                    }
                    break;
                case ItemColumns.ItemPrefab:
                    item.data.ItemPrefab = EditorGUI.ObjectField(cellRect,
                                                                 item.data.ItemPrefab,
                                                                 typeof(GameObject),
                                                                 false) as GameObject;
                    break;
                default:
                    break;
            }
        }
        else
        {
            GUIStyle style;
            switch (column)
            {
                case ItemColumns.ID:
                    EditorGUI.LabelField(cellRect, item.data.ElementID.ToString("D3"), EditorStyles.boldLabel);
                    break;
                case ItemColumns.Name:
                    style = EditorStyles.label;
                    style.fontStyle = FontStyle.BoldAndItalic;
                    EditorGUI.LabelField(cellRect, item.data.name, style);
                    break;
                case ItemColumns.Quote:
                    EditorGUI.LabelField(cellRect, item.data.ItemQuote, EditorStyles.wordWrappedLabel);
                    break;
                case ItemColumns.Description:
                    EditorGUI.LabelField(cellRect, item.data.ItemDescription, EditorStyles.wordWrappedLabel);
                    break;
                case ItemColumns.ItemSprite:
                    if (item.data.ItemSprite != null)
                        GUI.DrawTexture(cellRect, item.data.ItemSprite.texture, ScaleMode.ScaleToFit);
                    else
                    {
                        style = EditorStyles.label;
                        style.richText = true;
                        EditorGUI.LabelField(cellRect, "<color=red>Need texture here!</color>", style);
                    }
                    break;
                case ItemColumns.ItemPrefab:
                    item.data.ItemPrefab = EditorGUI.ObjectField(cellRect,
                                                                 item.data.ItemPrefab,
                                                                 typeof(GameObject),
                                                                 false) as GameObject;
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
