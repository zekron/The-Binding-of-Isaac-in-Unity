using System;
using UnityEngine;

public class RandomObjectBaseSO<T> : ScriptableObject, IDisplayInEditorWindow where T : RandomObjectData
{
    private const int MAX_RATIO = 100;
    public T[] RandomObjectArray;
    public Sprite SpriteInEditorWindow => spriteInEditorWindow;

    [SerializeField] private Sprite spriteInEditorWindow;

    private void OnValidate()
    {
        if (RandomObjectArray.Length <= 0) return;
        int tempRatio = 0;
        for (int i = 0; i < RandomObjectArray.Length; i++)
        {
            if (tempRatio + RandomObjectArray[i].ratio > MAX_RATIO)
                RandomObjectArray[i].ratio = MAX_RATIO - tempRatio;

            if (tempRatio == MAX_RATIO) continue;

            tempRatio += RandomObjectArray[i].ratio;
        }
    }

    public GameObject GenerateObject()
    {
        if (RandomObjectArray.Length <= 0) CustomDebugger.ThrowException("Wrong RandomPickupArray length.");

        RandomObjectArray.Shuffle();
        int rnd = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < RandomObjectArray.Length; i++)
        {
            if (RandomObjectArray[i].ratio >= rnd)
                return RandomObjectArray[i].RandomObject;
            else
                rnd -= RandomObjectArray[i].ratio;
        }

        return RandomObjectArray[0].RandomObject;
    }
}

[Serializable]
public class TupleWithRandomPickupCoordinate : CustomTuple<RandomObjectSO, GameCoordinate>
{
    public TupleWithRandomPickupCoordinate(RandomObjectSO item1, GameCoordinate item2) : base(item1, item2) { }
}
