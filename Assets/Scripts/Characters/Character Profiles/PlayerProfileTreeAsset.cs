using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProfileTreeAsset", menuName = "Scriptable Object/Tree Asset/PlayerProfile", order = 1)]
public class PlayerProfileTreeAsset : CharacterProfileTreeAsset<PlayerProfileTreeElement>
{
    public PlayerProfileTreeAsset()
    {
        m_TreeElements = new List<PlayerProfileTreeElement>()
        {
            new PlayerProfileTreeElement("Root",
                                         HealthData.RedOne,
                                         PickupData.zero,
                                         CollectableItemData.Empty,
                                         depth: -1,
                                         id: m_MaxID),
        };
        CreateProfile();
    }

    public override PlayerProfileTreeElement CreateProfile()
    {
        PlayerProfileTreeElement result = new PlayerProfileTreeElement("Name here",
                                                                       HealthData.RedOne,
                                                                       PickupData.zero,
                                                                       CollectableItemData.Empty,
                                                                       depth: 0,
                                                                       id: GenerateUniqueID());
        m_TreeElements.Add(result);
        return result;
    }
}