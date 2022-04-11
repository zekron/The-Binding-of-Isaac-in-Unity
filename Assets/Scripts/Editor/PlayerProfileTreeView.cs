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

    public PlayerProfileTreeView(TreeViewState state,
                                 MultiColumnHeader multicolumnHeader,
                                 TreeModel<PlayerProfileTreeElement> model) : base(state, multicolumnHeader, model)
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

                    item.data.PlayerHealthData.RefreshData(item.data.PlayerHealthData.RedHeart,
                                                           (int)GUI.HorizontalSlider(redHeartCellRect, item.data.PlayerHealthData.RedHeart, 0, 5),
                                                           (int)GUI.HorizontalSlider(soulHeartCellRect, item.data.PlayerHealthData.SoulHeart, 0, 5));

                    DefaultGUI.Label(redHeartLabelCellRect, item.data.PlayerHealthData.RedHeart.ToString("D"), args.selected, args.focused);
                    DefaultGUI.Label(soulHeartLabelCellRect, item.data.PlayerHealthData.SoulHeart.ToString("D"), args.selected, args.focused);
                    break;
                case MyColumns.BaseMoveSpeed:
                    //CustommathematicTextField(cellRect, ref item.data.BaseMoveSpeed);
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
                case MyColumns.StartingPickup:
                    baseWidth = cellRect.width / 3;
                    var coinCellRect = new Rect(cellRect.x, cellRect.y, baseWidth - labelPadding, cellRect.height);
                    var coinLabelCellRect = new Rect(cellRect.x + baseWidth - labelPadding / 2, cellRect.y, labelPadding, cellRect.height);
                    var keyHeartCellRect = new Rect(cellRect.x + baseWidth/* + labelPadding / 2*/, cellRect.y, baseWidth - labelPadding, cellRect.height);
                    var keyLabelCellRect = new Rect(cellRect.x + 2 * baseWidth - labelPadding / 2, cellRect.y, labelPadding, cellRect.height);

                    item.data.PlayerPickupData.RefreshData((int)GUI.HorizontalSlider(coinCellRect, item.data.PlayerPickupData.Coin, 0, 5),
                                                           (int)GUI.HorizontalSlider(coinCellRect, item.data.PlayerPickupData.Key, 0, 5),
                                                           (int)GUI.HorizontalSlider(keyHeartCellRect, item.data.PlayerPickupData.Bomb, 0, 5));

                    DefaultGUI.Label(coinLabelCellRect, item.data.PlayerPickupData.Coin.ToString("D"), args.selected, args.focused);
                    DefaultGUI.Label(keyLabelCellRect, item.data.PlayerPickupData.Key.ToString("D"), args.selected, args.focused);

                    break;
                case MyColumns.StartingItem:
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
                    GUI.DrawTexture(cellRect, GetHeartTexture(item.data.PlayerHealthData.RedHeart,
                                                              item.data.PlayerHealthData.SoulHeart), ScaleMode.ScaleToFit);
                    break;
                case MyColumns.StartingPickup:
                    GUI.DrawTexture(cellRect, GetPickupTexture(item.data.PlayerPickupData.Coin,
                                                               item.data.PlayerPickupData.Key,
                                                               item.data.PlayerPickupData.Bomb), ScaleMode.ScaleToFit);
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

    readonly Sprite redHeartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_RedHeart.png");
    readonly Sprite soulHeartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_SoulHeart.png");
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
    readonly Sprite coinSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_Coin.png");
    readonly Sprite keySprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_Key.png");
    readonly Sprite bombSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/Single/UI_Bomb.png");
    private Texture2D GetPickupTexture(int coinCount, int keyCount, int bombCount)
    {
        var rect = coinSprite.rect;
        int width = (int)rect.width;
        int height = (int)rect.height;
        Texture2D result = new Texture2D((coinCount + keyCount + bombCount) * width, height);

        var coinData = coinSprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);
        var keyData = keySprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);
        var bombData = bombSprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);

        for (int i = 0; i < coinCount + keyCount + bombCount; i++)
        {
            if (i < coinCount)
                result.SetPixels((int)rect.x + i * width, (int)rect.y, width, height, coinData);
            else if (i < coinCount + keyCount)
                result.SetPixels((int)rect.x + i * width, (int)rect.y, width, height, keyData);
            else
                result.SetPixels((int)rect.x + i * width, (int)rect.y, width, height, bombData);
        }

        result.Apply();
        return result;
    }
}
