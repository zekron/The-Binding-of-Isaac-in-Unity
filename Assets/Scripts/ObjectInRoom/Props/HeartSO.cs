using UnityEngine;

[CreateAssetMenu(fileName = "HeartSO", menuName = "Scriptable Object/Pickups/Heart")]
public class HeartSO : ScriptableObject
{
    public Sprite[] HeartSprites;

    public readonly static int[] HeartWorths = new int[] { 1, 0 };

    public enum HeartType
    {
        RedHalf,
        RedFull,

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
