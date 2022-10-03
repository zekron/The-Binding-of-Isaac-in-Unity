using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : PickupObject, ICollectInSlot
{
    [SerializeField] private CardSO cardSO;

    private CardSO.CardType cardType;
    private Sprite spriteInSlot;
    private string message;

    public string ObjectTitle => message;
    public string ObjectMessage => message;
    public Sprite SpriteInSlot => spriteInSlot;

    public void CollectInSlot()
    {
        //º§ªÓSlot
    }
    public void Activate()
    {
        switch (cardType)
        {
            case CardSO.CardType.The_Fool:
                break;
            case CardSO.CardType.The_Magician:
                break;
            case CardSO.CardType.The_HighPriestess:
                break;
            case CardSO.CardType.The_Empress:
                break;
            case CardSO.CardType.The_Emperor:
                break;
            case CardSO.CardType.The_Hierophant:
                break;
            case CardSO.CardType.The_Lovers:
                break;
            case CardSO.CardType.The_Chariot:
                break;
            case CardSO.CardType.Justice:
                break;
            case CardSO.CardType.The_Hermit:
                break;
            case CardSO.CardType.Wheel_of_Fortune:
                break;
            case CardSO.CardType.Strength:
                break;
            case CardSO.CardType.The_HangedMan:
                break;
            case CardSO.CardType.Death:
                break;
            case CardSO.CardType.Temperance:
                break;
            case CardSO.CardType.The_Devil:
                break;
            case CardSO.CardType.The_Tower:
                break;
            case CardSO.CardType.The_Stars:
                break;
            case CardSO.CardType.The_Moon:
                break;
            case CardSO.CardType.The_Sun:
                break;
            case CardSO.CardType.Judgement:
                break;
            case CardSO.CardType.The_World:
                break;
            case CardSO.CardType.Two_of_Spades:
                break;
            case CardSO.CardType.Two_of_Hearts:
                break;
            case CardSO.CardType.Two_of_Clubs:
                break;
            case CardSO.CardType.Two_of_Diamonds:
                break;
            case CardSO.CardType.Joker:
                break;
            default:
                break;
        }
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        cardType = cardSO.GenerateType();

        if (cardType < CardSO.CardType.Two_of_Spades)
            objectRenderer.sprite = cardSO.CardSpritesInRoom[0];
        else
            objectRenderer.sprite = cardSO.CardSpritesInRoom[1];

        spriteInSlot = cardSO.CardSpritesInSlot[(int)cardType];
        message = CardSO.CARD_MESSAGE[(int)cardType];
    }
    protected override void OnPlayerCollect()
    {
        CollectInSlot();

    }
}
