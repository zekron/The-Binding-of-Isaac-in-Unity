using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Object/Pickups/Card")]
public class CardSO : ScriptableObject
{
    public Sprite[] CardSpritesInSlot;
    public Sprite[] CardSpritesInRoom;

    public static readonly string[] CARD_MESSAGE = new string[]
    {
        "0 - The Fool",
        "I - The Magician",
        "II - The High Priestess",
        "III - The Empress",
        "IV - The Emperor",
        "V - The Hierophant",
        "VI - The Lovers",
        "VII - The Chariot",
        "VIII - Justice",
        "IX - The Hermit",
        "X - Wheel of Fortune",
        "XI - Strength",
        "XII - The Hanged Man",
        "XIII - Death",
        "XIV - Temperance",
        "XV - The Devil",
        "XVI - The Tower",
        "XVII - The Stars",
        "XVIII - The Moon",
        "XIX - The Sun",
        "XX - Judgement",
        "XXI - The World",
        "2 of Spades",
        "2 of Hearts",
        "2 of Clubs",
        "2 of Diamonds",
        "Joker",
    };

    public enum CardType
    {
        The_Fool,
        The_Magician,
        The_HighPriestess,
        The_Empress,
        The_Emperor,
        The_Hierophant,
        The_Lovers,
        The_Chariot,
        Justice,
        The_Hermit,
        Wheel_of_Fortune,
        Strength,
        The_HangedMan,
        Death,
        Temperance,
        The_Devil,
        The_Tower,
        The_Stars,
        The_Moon,
        The_Sun,
        Judgement,
        The_World,

        Two_of_Spades,
        Two_of_Hearts,
        Two_of_Clubs,
        Two_of_Diamonds,

        Joker,
    }

    public CardType GenerateType()
    {
        return (CardType)Random.Range(0, System.Enum.GetValues(typeof(CardType)).Length);
    }
}
