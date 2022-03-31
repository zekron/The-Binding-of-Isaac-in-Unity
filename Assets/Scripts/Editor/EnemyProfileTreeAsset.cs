using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfileTreeAsset", menuName = "Scriptable Object/Tree Asset/EnemyProfile", order = 1)]
public class EnemyProfileTreeAsset : CharacterProfileTreeAsset<EnemyProfileTreeElement>
{
    public EnemyProfileTreeAsset()
    {
        m_TreeElements = new List<EnemyProfileTreeElement>()
        {
            new EnemyProfileTreeElement("Root", 0, depth: -1, id: m_MaxID),
        };
        CreateProfile();
    }

    public override EnemyProfileTreeElement CreateProfile()
    {
        EnemyProfileTreeElement result = new EnemyProfileTreeElement("Name here", 0, depth: 0, id: GenerateUniqueID());
        m_TreeElements.Add(result);
        return result;
    }
}
