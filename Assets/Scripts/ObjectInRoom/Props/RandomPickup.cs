using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Object", menuName = "Scriptable Object/Random Object/Random Pickup")]
public class RandomPickup : ScriptableObject, IDisplayInEditorWindow
{
    public RamdomObjectData[] RandomPickupArray;
    public Sprite SpriteInEditorWindow => spriteInEditorWindow;

    [SerializeField] private Sprite spriteInEditorWindow;

    public GameObject Generate()
    {
        if (RandomPickupArray.Length <= 0) CustomDebugger.ThrowException("Wrong RandomPickupArray length.");

        RandomPickupArray.Shuffle();
        int rnd = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < RandomPickupArray.Length; i++)
        {
            if (RandomPickupArray[i].ratio >= rnd)
                return RandomPickupArray[i].RandomObject;
            else
                rnd -= RandomPickupArray[i].ratio;
        }

        return RandomPickupArray[0].RandomObject;
    }
}

[Serializable]
public class TupleWithRandomPickupCoordinate : CustomTuple<RandomPickup, GameCoordinate>
{
    public TupleWithRandomPickupCoordinate(RandomPickup item1, GameCoordinate item2) : base(item1, item2) { }
}
