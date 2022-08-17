using System.Collections.Generic;

public class CollectibleItemTreeAsset : ItemTreeAsset<CollectibleItemTreeElement>
{
    public CollectibleItemTreeAsset()
    {
        m_TreeElements = new List<CollectibleItemTreeElement>()
        {
            new CollectibleItemTreeElement(-1,
                                           "Root",
                                           depth: -1,
                                           "Null",
                                           null,
                                           null),
        };
        CreateProfile();
    }
    public override CollectibleItemTreeElement CreateProfile()
    {
        var result = new CollectibleItemTreeElement(id: GenerateUniqueID(),
                                                    "Name here",
                                                    depth: 0,
                                                    "Description here",
                                                    null,
                                                    null);
        m_TreeElements.Add(result);
        return result;
    }
}
