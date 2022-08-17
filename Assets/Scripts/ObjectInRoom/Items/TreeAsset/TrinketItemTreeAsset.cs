using System.Collections.Generic;

public class TrinketItemTreeAsset : ItemTreeAsset<ItemTreeElement>
{
    public TrinketItemTreeAsset()
    {
        m_TreeElements = new List<ItemTreeElement>()
        {
            new ItemTreeElement(-1,
                                "Root",
                                depth: -1,
                                "TreeRoot",
                                null),
        };
        CreateProfile();
    }
    public override ItemTreeElement CreateProfile()
    {
        ItemTreeElement result = new ItemTreeElement(id: GenerateUniqueID(),
                                                     "Name here",
                                                     depth: 0,
                                                     "Description here",
                                                     null);
        m_TreeElements.Add(result);
        return result;
    }
}
