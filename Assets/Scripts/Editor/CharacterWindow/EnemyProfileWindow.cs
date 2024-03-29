using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static CharacterProfileTreeView<PlayerProfileTreeElement>;

public class EnemyProfileWindow : ProfileWindow<EnemyProfileTreeElement>
{
    protected override IList<EnemyProfileTreeElement> GetData()
    {
        if (CheckTreeAsset())
            return m_MyTreeAsset.TreeElements;
        else
        {
            CheckFileExists(StaticData.ScriptableObjectFolderPath,
                            StaticData.FILE_ENEMYPROFILE_SO,
                            typeof(EnemyProfileTreeAsset));

            m_MyTreeAsset = AssetDatabase.LoadAssetAtPath<EnemyProfileTreeAsset>(Path.Combine(StaticData.ScriptableObjectFolderPath,
                                                                                              StaticData.FILE_ENEMYPROFILE_SO));

            EditorUtility.SetDirty(m_MyTreeAsset);
            return m_MyTreeAsset.TreeElements;
        }
    }

    protected override void AddPlayerProfile()
    {
        if (CheckTreeAsset())
        {
            m_TreeView.CustomTreeModel.AddElement(m_MyTreeAsset.CreateProfile(),
                                                  m_MyTreeAsset.TreeRoot,
                                                  Mathf.Max(0, m_MyTreeAsset.TreeRoot.Children.Count - 1));
            DoTreeView(MultiColumnTreeViewRect);
        }
        else Debug.Log("AddPlayerProfile");

        SetEditModeToggle(true);

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override void DeletePlayerProfile()
    {
        if (CheckTreeAsset())
        {
            IList<int> selections = m_TreeView.GetSelection();
            m_TreeView.CustomTreeModel.RemoveElements(selections);
        }
        else Debug.Log("DeletePlayerProfile");

        EditorUtility.SetDirty(m_MyTreeAsset);
    }

    protected override CharacterProfileTreeView<EnemyProfileTreeElement> CreateTreeView(MultiColumnHeader multiColumnHeader,
                                                                                        TreeModel<EnemyProfileTreeElement> treeModel)
    {
        return new EnemyProfileTreeView(m_TreeViewState, multiColumnHeader, treeModel);
    }

    protected override MultiColumnHeaderState GetHeaderState()
    {
        return CreateDefaultMultiColumnHeaderState(
            MyColumns.ID,
            MyColumns.Name,
            MyColumns.BaseHealth,
            MyColumns.BaseMoveSpeed,
            MyColumns.BaseDamage,
            MyColumns.BaseRange,
            MyColumns.TearsMultiplier);
    }
}
