﻿using UnityEditor.IMGUI.Controls;

public class TreeViewItem<T> : TreeViewItem where T : TreeElement
{
    public T data { get; set; }

    public TreeViewItem(int id, int depth, string displayName, T data) : base(id, depth, displayName)
    {
        this.data = data;
    }
}
