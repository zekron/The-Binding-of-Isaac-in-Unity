using UnityEngine;

[CreateAssetMenu(fileName = "HeartSO", menuName = "Scriptable Object/Pickups/Heart")]
public class HeartSO : ScriptableObject
{
    public Sprite[] HeartSprites;
    public Sprite[] HeartSpritesInUI;

    public readonly static HealthData[] HeartWorths = new HealthData[]
    {
        HealthData.RedHalf,
        HealthData.RedOne,
        HealthData.SoulOne,
        HealthData.WhiteHalf,
    };

    public enum HeartType
    {
        RedHalf,
        RedFull,

        SoulFull,

        WhiteHalf,
    }

    public enum HeartTypeInUI
    {
        NULL = -1,

        RedEmpty,
        RedHalf,
        RedFull,

        SoulHalf,
        SoulFull,

        WhiteHalf,
    }

    public HeartType GenerateType()
    {
        var rnd = Random.value;

        if (rnd < 7f / 10) return HeartType.RedHalf;
        else return HeartType.RedFull;
    }
}
