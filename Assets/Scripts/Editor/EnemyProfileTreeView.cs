using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyProfileTreeView : CharacterProfileTreeView<EnemyProfileTreeElement>
{
    enum SortOption
    {
        ID,
        Name,
        BaseHealth,
        BaseMoveSpeed,
        BaseDamage,
        BaseRange,
        TearDelay,
    }
    public EnemyProfileTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, TreeModel<EnemyProfileTreeElement> model) : base(state, multiColumnHeader, model)
    {
    }

    protected override void CellGUI(Rect cellRect, TreeViewItem<EnemyProfileTreeElement> item, MyColumns column, ref RowGUIArgs args)
    {
        CenterRectUsingSingleLineHeight(ref cellRect);

        string value = "Missing";

        if (InEditMode)
        {
            float result;
            switch (column)
            {
                case MyColumns.ID:
                    value = item.data.ElementID.ToString("D4");
                    DefaultGUI.Label(cellRect, value, args.selected, args.focused);
                    break;
                case MyColumns.Name:
                    item.data.name = GUI.TextField(cellRect, item.data.name);
                    break;
                case MyColumns.BaseHealth:
                    break;
                case MyColumns.BaseMoveSpeed:
                    value = GUI.TextField(cellRect, item.data.BaseMoveSpeed.ToString());
                    if (float.TryParse(value, out result))
                        item.data.BaseMoveSpeed = result;
                    else
                        GUI.TextField(cellRect, item.data.BaseMoveSpeed.ToString());
                    break;
                case MyColumns.BaseDamage:
                    value = GUI.TextField(cellRect, item.data.BaseDamage.ToString());
                    if (float.TryParse(value, out result))
                        item.data.BaseDamage = result;
                    else
                        GUI.TextField(cellRect, item.data.BaseDamage.ToString());
                    break;
                case MyColumns.BaseRange:
                    value = GUI.TextField(cellRect, item.data.BaseRange.ToString());
                    if (float.TryParse(value, out result))
                        item.data.BaseRange = result;
                    else
                        GUI.TextField(cellRect, item.data.BaseRange.ToString());
                    break;
                case MyColumns.TearsMultiplier:
                    value = GUI.TextField(cellRect, item.data.TearDelay.ToString());
                    if (float.TryParse(value, out result))
                        item.data.TearDelay = result;
                    else
                        GUI.TextField(cellRect, item.data.TearDelay.ToString());
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (column)
            {
                case MyColumns.ID:
                    value = item.data.ElementID.ToString("D4");
                    DefaultGUI.Label(cellRect, value, args.selected, args.focused);
                    break;
                case MyColumns.Name:
                    value = item.data.name;
                    DefaultGUI.BoldLabelRightAligned(cellRect, value, args.selected, args.focused);
                    break;
                case MyColumns.BaseHealth:
                case MyColumns.BaseMoveSpeed:
                case MyColumns.BaseDamage:
                case MyColumns.BaseRange:
                case MyColumns.TearsMultiplier:
                    if (column == MyColumns.BaseMoveSpeed)
                        value = item.data.BaseMoveSpeed.ToString("f2");
                    else if (column == MyColumns.BaseDamage)
                        value = item.data.BaseDamage.ToString("f2");
                    else if (column == MyColumns.BaseRange)
                        value = item.data.BaseRange.ToString("f2");
                    else if (column == MyColumns.TearsMultiplier)
                        value = item.data.TearDelay.ToString("f2");

                    DefaultGUI.LabelRightAligned(cellRect, value, args.selected, args.focused);
                    break;
                default:
                    break;
            }
        }
    }

    internal static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
    {
        var columns = new[]
        {
            new MultiColumnHeaderState.Column
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
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Character Name", "Name of character"),
                contextMenuText = "Type",
                headerTextAlignment = TextAlignment.Left,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Right,
                width = 130,
                minWidth = 100,
                maxWidth = 200,
                autoResize = false,
                allowToggleVisibility = true
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Base Health"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 200,
                minWidth = 60,
                autoResize = false,
                allowToggleVisibility = false
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Base Speed"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 150,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = false
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Base Damage"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 150,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = false
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Base Range", "How far the tears go"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 95,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = true
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Tears Delay", "Real mechanic about Tears, with formula"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 70,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = true
            }
        };
        
        Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

        var state = new MultiColumnHeaderState(columns);
        return state;
    }

    protected override IOrderedEnumerable<TreeViewItem<EnemyProfileTreeElement>> InitialOrder(IEnumerable<TreeViewItem<EnemyProfileTreeElement>> myTypes, int[] history)
    {
        bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
        switch ((SortOption)history[0])
        {
            case SortOption.ID:
                return myTypes.Order(l => l.data.ElementID, ascending);
            case SortOption.Name:
                return myTypes.Order(l => l.data.name, ascending);
            case SortOption.BaseHealth:
                return myTypes.Order(l => l.data.BaseHealth, ascending);
            case SortOption.BaseMoveSpeed:
                return myTypes.Order(l => l.data.BaseMoveSpeed, ascending);
            case SortOption.BaseDamage:
                return myTypes.Order(l => l.data.BaseDamage, ascending);
            case SortOption.BaseRange:
                return myTypes.Order(l => l.data.BaseRange, ascending);
            case SortOption.TearDelay:
                return myTypes.Order(l => l.data.TearDelay, ascending);
            default:
                Assert.IsTrue(false, "Unhandled enum");
                break;
        }

        // default
        return myTypes.Order(l => l.data.name, ascending);
    }

    protected override void SortByMultipleColumns()
    {
        var sortedColumns = multiColumnHeader.state.sortedColumns;

        if (sortedColumns.Length == 0)
            return;

        var myTypes = rootItem.children.Cast<TreeViewItem<EnemyProfileTreeElement>>();
        var orderedQuery = InitialOrder(myTypes, sortedColumns);
        for (int i = 1; i < sortedColumns.Length; i++)
        {
            bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

            switch ((SortOption)sortedColumns[i])
            {
                case SortOption.Name:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
                    break;
                case SortOption.BaseHealth:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseHealth, ascending);
                    break;
                case SortOption.BaseMoveSpeed:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseMoveSpeed, ascending);
                    break;
                case SortOption.BaseDamage:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseDamage, ascending);
                    break;
                case SortOption.BaseRange:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseRange, ascending);
                    break;
                case SortOption.TearDelay:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.TearDelay, ascending);
                    break;
                case SortOption.ID:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.ElementID, ascending);
                    break;
            }
        }

        rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
    }
}
