using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class CharacterProfileTreeView<T> : TreeViewWithTreeModel<T> where T : CharacterProfileTreeElement
{
    protected const float ROW_HEIGHT = 20F;
    protected const float TOGGLE_WIDTH = 18f;

    protected MultiColumnHeader m_MultiColumnHeader;

    internal bool InEditMode = true;
    // All columns
    internal protected enum MyColumns
    {
        ID,
        Name,

        //Player
        BaseHealth,
        BaseMoveSpeed,
        BaseDamage,
        DamageMultiplier,
        BaseRange,
        Tears,
        TearDelay,
        ShotSpeed,
        Luck,
        StartingPickup,
        StartingItem,
    }

    public CharacterProfileTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, TreeModel<T> model) : base(state, multiColumnHeader, model)
    {
        // Custom setup
        rowHeight = ROW_HEIGHT;
        columnIndexForTreeFoldouts = 0;
        showAlternatingRowBackgrounds = true;
        showBorder = true;
        customFoldoutYOffset = (ROW_HEIGHT - EditorGUIUtility.singleLineHeight) * 0.5f; // center foldout in the row since we also center content. See RowGUI
        extraSpaceBeforeIconAndLabel = TOGGLE_WIDTH;
        m_MultiColumnHeader = multiColumnHeader;
        m_MultiColumnHeader.sortingChanged += OnSortingChanged;

        Reload();
    }

    protected override bool CanMultiSelect(TreeViewItem item)
    {
        return true;
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
        var rows = base.BuildRows(root);
        SortIfNeeded(root, rows);
        return rows;
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        var item = (TreeViewItem<T>)args.item;

        for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
        {
            CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
        }
    }

    private void OnSortingChanged(MultiColumnHeader multiColumnHeader)
    {
        SortIfNeeded(rootItem, GetRows());
        Repaint();
    }

    private void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
    {
        if (rows.Count <= 1)
            return;

        if (multiColumnHeader.sortedColumnIndex == -1)
        {
            return; // No column to sort for (just use the order the data are in)
        }

        // Sort the roots of the existing tree items
        SortByMultipleColumns();
        TreeToList(root, rows);
        Repaint();
    }

    internal static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(params MyColumns[] contents)
    {
        var columns = new MultiColumnHeaderState.Column[contents.Length];
        GUIContent tempHeaderContent = new GUIContent();
        for (int i = 0; i < columns.Length; i++)
        {
            switch (contents[i])
            {
                case MyColumns.ID:
                    tempHeaderContent = new GUIContent("Character ID", "ID of character");
                    break;
                case MyColumns.Name:
                    tempHeaderContent = new GUIContent("Character Name", "Name of character");
                    break;
                case MyColumns.BaseHealth:
                    tempHeaderContent = new GUIContent("Base Health");
                    break;
                case MyColumns.BaseMoveSpeed:
                    tempHeaderContent = new GUIContent("Base Speed");
                    break;
                case MyColumns.BaseDamage:
                    tempHeaderContent = new GUIContent("Base Damage");
                    break;
                case MyColumns.DamageMultiplier:
                    tempHeaderContent = new GUIContent("Damage Multiplier", "Will be used after Damage Formula");
                    break;
                case MyColumns.BaseRange:
                    tempHeaderContent = new GUIContent("Base Range", "How far the tears go");
                    break;
                case MyColumns.Tears:
                    tempHeaderContent = new GUIContent("Tears", "How many tears spawn per second");
                    break;
                case MyColumns.TearDelay:
                    tempHeaderContent = new GUIContent("Tears Delay", "Real mechanic about Tears, with formula");
                    break;
                case MyColumns.ShotSpeed:
                    tempHeaderContent = new GUIContent("Shot Speed", "Tear's speed");
                    break;
                case MyColumns.Luck:
                    tempHeaderContent = new GUIContent("Luck", "Affect a lot");
                    break;
                case MyColumns.StartingPickup:
                    tempHeaderContent = new GUIContent("StartingPickup");
                    break;
                case MyColumns.StartingItem:
                    tempHeaderContent = new GUIContent("StartingItem");
                    break;
                default:
                    tempHeaderContent = new GUIContent();
                    break;
            }

            if (contents[i] == MyColumns.ID || contents[i] == MyColumns.Name)
            {
                columns[i] = new MultiColumnHeaderState.Column
                {
                    headerContent = tempHeaderContent,
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
            }
            else if (contents[i] == MyColumns.BaseHealth || contents[i] == MyColumns.StartingPickup)
            {
                columns[i] = new MultiColumnHeaderState.Column
                {
                    headerContent = tempHeaderContent,
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 200,
                    minWidth = 200,
                    maxWidth = 300,
                    autoResize = false,
                    allowToggleVisibility = true
                };
            }
            else
            {
                columns[i] = new MultiColumnHeaderState.Column
                {
                    headerContent = tempHeaderContent,
                    contextMenuText = "Type",
                    headerTextAlignment = TextAlignment.Right,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Left,
                    width = 100,
                    minWidth = 60,
                    maxWidth = 200,
                    autoResize = false,
                    allowToggleVisibility = true
                };
            }
        }

        var state = new MultiColumnHeaderState(columns);
        return state;
    }

    string text;
    protected void CustommathematicTextField(Rect rect, ref float digit)
    {
        //TODO: TextField Calculator
        //text = GUI.TextField(rect, digit.ToString());
        //bool getDot = false; int dotIndex = 0;
        //float result = 0;

        //for (int i = 0; i < text.Length; i++)
        //{
        //    switch (text[i])
        //    {
        //        case '.':
        //            if (!getDot)
        //            {
        //                getDot = true;
        //                dotIndex = i;
        //            }
        //            else
        //            {
        //                text.Remove(i); i--;
        //            }
        //            break;
        //        case '1':
        //        case '2':
        //        case '3':
        //        case '4':
        //        case '5':
        //        case '6':
        //        case '7':
        //        case '8':
        //        case '9':
        //        case '0':
        //            result = result * 10 + int.Parse(text[i].ToString()); break;
        //        default:
        //            break;
        //    }
        //}
        //if (getDot)
        //{
        //    result = Mathf.Pow(0.1f, dotIndex-1);
        //}
        //digit = result;
    }

    protected abstract void SortByMultipleColumns();

    protected abstract void CellGUI(Rect cellRect, TreeViewItem<T> item, MyColumns column, ref RowGUIArgs args);

    protected abstract IOrderedEnumerable<TreeViewItem<T>> InitialOrder(IEnumerable<TreeViewItem<T>> myTypes, int[] history);
}