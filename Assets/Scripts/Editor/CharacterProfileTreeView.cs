using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class CharacterProfileTreeView<T> : TreeViewWithTreeModel<T> where T : CharacterProfileTreeElement
{
    protected const float ROW_HEIGHT = 20F;
    protected const float TOGGLE_WIDTH = 18f;

    protected MultiColumnHeader m_MultiColumnHeader;

    internal bool InEditMode = true;
    // All columns
    protected enum MyColumns
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
        Luck
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

    protected abstract void SortByMultipleColumns();

    protected abstract void CellGUI(Rect cellRect, TreeViewItem<T> item, MyColumns column, ref RowGUIArgs args);

    protected abstract IOrderedEnumerable<TreeViewItem<T>> InitialOrder(IEnumerable<TreeViewItem<T>> myTypes, int[] history);
}