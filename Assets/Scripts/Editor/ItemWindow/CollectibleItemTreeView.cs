using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CollectibleItemTreeView : ItemTreeView<CollectibleItemTreeElement>
{
    enum SortOption
    {
        ID, Name
    }
    public CollectibleItemTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, TreeModel<CollectibleItemTreeElement> model) : base(state, multiColumnHeader, model) { }
    
    protected override void CellGUI(Rect cellRect, TreeViewItem<CollectibleItemTreeElement> item, ItemColumns column, ref RowGUIArgs args)
    {
        throw new System.NotImplementedException();
    }

    protected override IOrderedEnumerable<TreeViewItem<CollectibleItemTreeElement>> InitialOrder(IEnumerable<TreeViewItem<CollectibleItemTreeElement>> myTypes, int[] history)
    {
        throw new System.NotImplementedException();
    }

    protected override void SortByMultipleColumns()
    {
        throw new System.NotImplementedException();
    }
}
