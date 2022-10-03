using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : PickupObject, ICollectInSlot
{
    [SerializeField] private PillSO pillSO;

    private PillSO.PillType pillType;
    private Sprite spriteInSlot;
    private string message;

    public string ObjectTitle => "? ? ?";//TODO: PhD
    public string ObjectMessage => message;
    public Sprite SpriteInSlot => spriteInSlot;

    public void CollectInSlot()
    {
        //激活Slot
    }

    public void Activate()
    {
        switch (pillType)
        {
            case PillSO.PillType.Bas_Gas:
                CustomDebugger.Log("Isaac farts, dealing 5 damage and poisoning enemies around him.");
                break;
            case PillSO.PillType.Bad_Trip:
                CustomDebugger.Log("Injures Isaac by a full heart. If a Bad Trip pill is used while Isaac only has a single heart left or less, the pill will turn into a Full Health pill instead.");
                break;
            case PillSO.PillType.Balls_of_Steel:
                CustomDebugger.Log("Grants Isaac two soul hearts.");
                break;
            case PillSO.PillType.Bombs_Are_Key:
                CustomDebugger.Log("Swaps the values of Isaac's Bomb Bombs and Key Keys.");
                break;
            case PillSO.PillType.Explosive_Diarrhea:
                CustomDebugger.Log("Spawns five active bombs behind Isaac over a period of five seconds, at one a second.");
                break;
            case PillSO.PillType.Full_Health:
                gamePlayer.FullHealth();
                break;
            case PillSO.PillType.Health_Down:
                gamePlayer.HealthDown();
                break;
            case PillSO.PillType.Health_Up:
                gamePlayer.HealthUp();
                break;
            case PillSO.PillType.I_Found_Pills:
                CustomDebugger.Log("Changes the appearance of Isaac's face. This pill is cosmetic only,");
                break;
            case PillSO.PillType.Puberty:
                CustomDebugger.Log("Causes Isaac to grow four hairs from the top of his head and zits on his face. This pill is cosmetic only and will stay for the rest of the run.");
                break;
            case PillSO.PillType.Pretty_Fly:
                CustomDebugger.Log("Grants a fly orbital, up to 3 maximum, that circles the player, blocking projectiles and damaging fly-type enemies on touch.");
                break;
            case PillSO.PillType.Range_Down:
                gamePlayer.Range -= 2;
                break;
            case PillSO.PillType.Range_Up:
                gamePlayer.Range += 2.5f;
                break;
            case PillSO.PillType.Speed_Down:
                gamePlayer.MoveSpeed -= 0.12f;
                break;
            case PillSO.PillType.Speed_Up:
                gamePlayer.MoveSpeed += 0.15f;
                break;
            case PillSO.PillType.Tears_Down:
                gamePlayer.Tears -= 0.28f;
                break;
            case PillSO.PillType.Tears_Up:
                gamePlayer.Tears += 0.35f;
                break;
            case PillSO.PillType.Luck_Down:
                gamePlayer.Luck -= 1;
                break;
            case PillSO.PillType.Luck_Up:
                gamePlayer.Luck += 1;
                break;
            case PillSO.PillType.Telepills:
                CustomDebugger.Log("Isaac gets teleported to a random room. Has a small chance to teleport Isaac to the I AM ERROR I AM ERROR room,");
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

        pillType = pillSO.GenerateType(out spriteInSlot);
        objectRenderer.sprite = spriteInSlot;
    }

    protected override void OnPlayerCollect()
    {
        CollectInSlot();

    }
}
