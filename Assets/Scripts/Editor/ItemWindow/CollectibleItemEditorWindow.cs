using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CollectibleItemEditorWindow : ItemEditorWindow<CollectibleItemTreeElement>
{
    protected override void AddPlayerProfile()
    {
        throw new System.NotImplementedException();
    }

    protected override ItemTreeView<CollectibleItemTreeElement> CreateTreeView(MultiColumnHeader multiColumnHeader, TreeModel<CollectibleItemTreeElement> treeModel)
    {
        return new CollectibleItemTreeView(treeViewState, multiColumnHeader, treeModel);
    }

    protected override void DeletePlayerProfile()
    {
        throw new System.NotImplementedException();
    }

    protected override IList<CollectibleItemTreeElement> GetData()
    {
        throw new System.NotImplementedException();
    }

    protected override MultiColumnHeaderState GetHeaderState()
    {
        throw new System.NotImplementedException();
    }
}
