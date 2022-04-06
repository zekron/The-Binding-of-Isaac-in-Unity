using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;
public class PlayerProfileTreeView : CharacterProfileTreeView<PlayerProfileTreeElement>
{
    enum SortOption
    {
        ID,
        Name,
        BaseHealth,
        BaseMoveSpeed,
        BaseDamage,
        DamageMultiplier,
        BaseRange,
        Tears,
        TearDelay,
        ShotSpeed,
        Luck
    }

    public PlayerProfileTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<PlayerProfileTreeElement> model) : base(state, multicolumnHeader, model)
    {
    }

    //protected override void RowGUI(RowGUIArgs args)
    //{
    //    var item = (TreeViewItem<PlayerProfileTreeElement>)args.item;

    //    for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
    //    {
    //        CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
    //    }
    //}

    protected override void CellGUI(Rect cellRect, TreeViewItem<PlayerProfileTreeElement> item, MyColumns column, ref RowGUIArgs args)
    {// Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
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
                    float baseWidth = cellRect.width / 2;
                    const int labelPadding = 30;
                    var redHeartCellRect = new Rect(cellRect.x, cellRect.y, baseWidth - labelPadding, cellRect.height);
                    var redHeartLabelCellRect = new Rect(cellRect.x + baseWidth - labelPadding / 2, cellRect.y, labelPadding, cellRect.height);
                    var soulHeartCellRect = new Rect(cellRect.x + baseWidth/* + labelPadding / 2*/, cellRect.y, baseWidth - labelPadding, cellRect.height);
                    var soulHeartLabelCellRect = new Rect(cellRect.x + 2 * baseWidth - labelPadding / 2, cellRect.y, labelPadding, cellRect.height);

                    item.data.PlayerHealthData.Initialze((int)GUI.HorizontalSlider(redHeartCellRect, item.data.PlayerHealthData.RedHeart, 0, 5),
                                                         (int)GUI.HorizontalSlider(soulHeartCellRect, item.data.PlayerHealthData.SoulHeart, 0, 5));

                    DefaultGUI.Label(redHeartLabelCellRect, item.data.PlayerHealthData.RedHeart.ToString("D"), args.selected, args.focused);
                    DefaultGUI.Label(soulHeartLabelCellRect, item.data.PlayerHealthData.SoulHeart.ToString("D"), args.selected, args.focused);
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
                case MyColumns.DamageMultiplier:
                    value = GUI.TextField(cellRect, item.data.DamageMultiplier.ToString());
                    if (float.TryParse(value, out result))
                        item.data.DamageMultiplier = result;
                    else
                        GUI.TextField(cellRect, item.data.DamageMultiplier.ToString());
                    break;
                case MyColumns.BaseRange:
                    value = GUI.TextField(cellRect, item.data.BaseRange.ToString());
                    if (float.TryParse(value, out result))
                        item.data.BaseRange = result;
                    else
                        GUI.TextField(cellRect, item.data.BaseRange.ToString());
                    break;
                case MyColumns.Tears:
                    value = GUI.TextField(cellRect, item.data.Tears.ToString());
                    if (float.TryParse(value, out result))
                        item.data.Tears = result;
                    else
                        GUI.TextField(cellRect, item.data.Tears.ToString());
                    break;
                case MyColumns.TearDelay:
                    value = GUI.TextField(cellRect, item.data.TearDelay.ToString());
                    if (float.TryParse(value, out result))
                        item.data.TearDelay = result;
                    else
                        GUI.TextField(cellRect, item.data.TearDelay.ToString());
                    break;
                case MyColumns.ShotSpeed:
                    value = GUI.TextField(cellRect, item.data.ShotSpeed.ToString());
                    if (float.TryParse(value, out result))
                        item.data.ShotSpeed = result;
                    else
                        GUI.TextField(cellRect, item.data.ShotSpeed.ToString());
                    break;
                case MyColumns.Luck:
                    value = GUI.TextField(cellRect, item.data.Luck.ToString());
                    if (float.TryParse(value, out result))
                        item.data.Luck = result;
                    else
                        GUI.TextField(cellRect, item.data.Luck.ToString());
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (column)
            {
                case MyColumns.BaseHealth:
                    value = item.data.PlayerHealthData.RedHeart.ToString("D");
                    GUI.DrawTexture(cellRect, GetHeartTexture(item.data.PlayerHealthData.RedHeart, item.data.PlayerHealthData.SoulHeart), ScaleMode.ScaleToFit);
                    break;
                case MyColumns.ID:
                    value = item.data.ElementID.ToString("D4");
                    DefaultGUI.Label(cellRect, value, args.selected, args.focused);
                    break;
                case MyColumns.Name:
                    value = item.data.name;
                    DefaultGUI.BoldLabelRightAligned(cellRect, value, args.selected, args.focused);
                    break;
                case MyColumns.BaseMoveSpeed:
                case MyColumns.BaseDamage:
                case MyColumns.DamageMultiplier:
                case MyColumns.BaseRange:
                case MyColumns.Tears:
                case MyColumns.TearDelay:
                case MyColumns.ShotSpeed:
                case MyColumns.Luck:
                    if (column == MyColumns.BaseMoveSpeed)
                        value = item.data.BaseMoveSpeed.ToString("f2");
                    else if (column == MyColumns.BaseDamage)
                        value = item.data.BaseDamage.ToString("f2");
                    else if (column == MyColumns.DamageMultiplier)
                        value = item.data.DamageMultiplier.ToString("f2");
                    else if (column == MyColumns.BaseRange)
                        value = item.data.BaseRange.ToString("f2");
                    else if (column == MyColumns.Tears)
                        value = item.data.Tears.ToString("f2");
                    else if (column == MyColumns.TearDelay)
                        value = item.data.TearDelay.ToString("f2");
                    else if (column == MyColumns.ShotSpeed)
                        value = item.data.ShotSpeed.ToString("f2");
                    else
                        value = item.data.Luck.ToString("f2");

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
                headerContent = new GUIContent("Damage Multiplier", "Will be used after Damage Formula"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 110,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = true
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
                headerContent = new GUIContent("Tears", "How many tears spawn per second"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 70,
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
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Shot Speed", "Tear's speed"),
                headerTextAlignment = TextAlignment.Right,
                sortedAscending = true,
                sortingArrowAlignment = TextAlignment.Left,
                width = 70,
                minWidth = 60,
                autoResize = true,
                allowToggleVisibility = true
            },
            new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Luck", "Affect a lot"),
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

    //private void OnSortingChanged(MultiColumnHeader multiColumnHeader)
    //{
    //    SortIfNeeded(rootItem, GetRows());
    //    Repaint();
    //}

    //private void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
    //{
    //    if (rows.Count <= 1)
    //        return;

    //    if (multiColumnHeader.sortedColumnIndex == -1)
    //    {
    //        return; // No column to sort for (just use the order the data are in)
    //    }

    //    // Sort the roots of the existing tree items
    //    SortByMultipleColumns();
    //    TreeToList(root, rows);
    //    Repaint();
    //}

    protected override void SortByMultipleColumns()
    {
        var sortedColumns = multiColumnHeader.state.sortedColumns;

        if (sortedColumns.Length == 0)
            return;

        var myTypes = rootItem.children.Cast<TreeViewItem<PlayerProfileTreeElement>>();
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
                    orderedQuery = orderedQuery.ThenBy(l => l.data.PlayerHealthData, ascending);
                    break;
                case SortOption.BaseMoveSpeed:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseMoveSpeed, ascending);
                    break;
                case SortOption.BaseDamage:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseDamage, ascending);
                    break;
                case SortOption.DamageMultiplier:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.DamageMultiplier, ascending);
                    break;
                case SortOption.BaseRange:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.BaseRange, ascending);
                    break;
                case SortOption.Tears:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.Tears, ascending);
                    break;
                case SortOption.TearDelay:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.TearDelay, ascending);
                    break;
                case SortOption.ShotSpeed:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.ShotSpeed, ascending);
                    break;
                case SortOption.Luck:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.Luck, ascending);
                    break;
                case SortOption.ID:
                    orderedQuery = orderedQuery.ThenBy(l => l.data.ElementID, ascending);
                    break;
            }
        }

        rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();
    }

    protected override IOrderedEnumerable<TreeViewItem<PlayerProfileTreeElement>> InitialOrder(IEnumerable<TreeViewItem<PlayerProfileTreeElement>> myTypes, int[] history)
    {
        bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
        switch ((SortOption)history[0])
        {
            case SortOption.ID:
                return myTypes.Order(l => l.data.ElementID, ascending);
            case SortOption.Name:
                return myTypes.Order(l => l.data.name, ascending);
            case SortOption.BaseHealth:
                return myTypes.Order(l => l.data.PlayerHealthData, ascending);
            case SortOption.BaseMoveSpeed:
                return myTypes.Order(l => l.data.BaseMoveSpeed, ascending);
            case SortOption.BaseDamage:
                return myTypes.Order(l => l.data.BaseDamage, ascending);
            case SortOption.DamageMultiplier:
                return myTypes.Order(l => l.data.DamageMultiplier, ascending);
            case SortOption.BaseRange:
                return myTypes.Order(l => l.data.BaseRange, ascending);
            case SortOption.Tears:
                return myTypes.Order(l => l.data.Tears, ascending);
            case SortOption.TearDelay:
                return myTypes.Order(l => l.data.TearDelay, ascending);
            case SortOption.ShotSpeed:
                return myTypes.Order(l => l.data.ShotSpeed, ascending);
            case SortOption.Luck:
                return myTypes.Order(l => l.data.Luck, ascending);
            default:
                Assert.IsTrue(false, "Unhandled enum");
                break;
        }

        // default
        return myTypes.Order(l => l.data.name, ascending);
    }

    Sprite redHeartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_RedHeart.png");
    Sprite soulHeartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_SoulHeart.png");
    private Texture2D GetHeartTexture(int redHeartCount, int soulHeartCount)
    {
        var rect = redHeartSprite.rect;
        int width = (int)rect.width;
        int height = (int)rect.height;
        Texture2D result = new Texture2D((redHeartCount + soulHeartCount) * width, height);

        var redData = redHeartSprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);
        var soulData = soulHeartSprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);

        for (int i = 0; i < redHeartCount + soulHeartCount; i++)
        {
            result.SetPixels((int)rect.x + i * width, (int)rect.y, width, height, i < redHeartCount ? redData : soulData);
        }

        result.Apply();
        return result;
    }
}
