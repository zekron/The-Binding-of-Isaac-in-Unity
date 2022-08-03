using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PillSO", menuName = "Scriptable Object/Pickups/Pill")]
public class PillSO : ScriptableObject
{
    public Sprite[] PillSprites;

    public enum PillType
    {
        Bas_Gas,
        Bad_Trip,
        Balls_of_Steel,
        Bombs_Are_Key,
        Explosive_Diarrhea,
        Full_Health,
        Health_Down,
        Health_Up,
        I_Found_Pills,
        Puberty,
        Pretty_Fly,
        Range_Down,
        Range_Up,
        Speed_Down,
        Speed_Up,
        Tears_Down,
        Tears_Up,
        Luck_Down,
        Luck_Up,
        Telepills,

    }
    private static PillType[] PillPool = new PillType[]
    {
        PillType.Bas_Gas,
        PillType.Bad_Trip,
        PillType.Balls_of_Steel,
        PillType.Bombs_Are_Key,
        PillType.Explosive_Diarrhea,
        PillType.Full_Health,
        PillType.Health_Down,
        PillType.Health_Up,
        PillType.I_Found_Pills,
        PillType.Puberty,
        PillType.Pretty_Fly,
        PillType.Range_Down,
        PillType.Range_Up,
        PillType.Speed_Down,
        PillType.Speed_Up,
        PillType.Tears_Down,
        PillType.Tears_Up,
        PillType.Luck_Down,
        PillType.Luck_Up,
        PillType.Telepills,
    };

    public static void GeneratePillPool()
    {
        PillPool.Shuffle();
    }

    public PillType GenerateType(out Sprite pillSprite)
    {
        int value = UnityEngine.Random.Range(0, PillSprites.Length);
        pillSprite = PillSprites[value];
        return PillPool[value];
    }
}
