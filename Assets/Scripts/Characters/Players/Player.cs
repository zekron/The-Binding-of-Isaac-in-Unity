using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private HealthDataEventChannelSO onPlayerHealthDataChanged;
    [SerializeField] private PickupDataEventChannelSO onPlayerPickupDataChanged;
    [SerializeField] private BooleanEventChannelSO onPlayerGetGoldenKey;

    private PlayerProfileTreeElement playerProfile;
    private PlayerController moveController;

    #region Current player status
    private HealthData currentHealth;
    private PickupData currentPickup;

    private float tearAddition = 0;
    private float shotSpeedAddition = 0;
    private float moveSpeedAddition = 0;
    private float luckAddition = 0;
    private float rangeAddition = 0;
    #endregion

    public int CoinCount => currentPickup.coin;
    public int KeyCount => currentPickup.key;
    public int BombCount => currentPickup.bomb;
    public bool GetGoldenKey => currentPickup.getGoldenKey;

    public float Tears
    {
        get => playerProfile.GetTears(PlayerProfileTreeElement.GetTearDelay(tearAddition));
        set => tearAddition += value;
    }
    public float ShotSpeed
    {
        get => playerProfile.ShotSpeed + shotSpeedAddition;
        set => shotSpeedAddition += value;
    }
    public float MoveSpeed
    {
        get => playerProfile.BaseMoveSpeed + moveSpeedAddition;
        set => moveSpeedAddition += value;
    }
    public float Luck
    {
        get => playerProfile.Luck + luckAddition;
        set => luckAddition += value;
    }
    public float Range
    {
        get => playerProfile.BaseRange + rangeAddition;
        set => rangeAddition += value;
    }

    private void Awake()
    {
        Initialize((int)playerCharacter);
        currentHealth = playerProfile.PlayerHealthData;
        currentPickup = playerProfile.PlayerPickupData;
        moveController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }

    private void Initialize(int id)
    {
        playerProfile = GameMgr.Instance.GetPlayerProfileByID(id);
    }

    #region Health
    public bool IsFullHealth()
    {
        return currentHealth.RedHeart == currentHealth.RedHeartContainers * 2;
    }
    public void GetHealing(HealthData data)
    {
        currentHealth += data;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
    }
    public void FullHealth()
    {
        currentHealth.RedHeart = currentHealth.RedHeartContainers * 2;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
    }
    public void HealthUp()
    {
        currentHealth.RedHeartContainers++;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
    }
    public void HealthDown()
    {
        if (currentHealth.RedHeart <= 2) currentHealth.RedHeartContainers++;
        else currentHealth.RedHeart -= 2;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
    }
    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);

        if (currentHealth == HealthData.Zero) GetDie();
    }
    public void SacrificeHealth(HealthData data)
    {
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);
    }
    #endregion

    #region BasicPickup
    public void GetCoin(int coinWorth)
    {
        currentPickup.coin += coinWorth;
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }
    public void UseCoin(int coinUse)
    {
        currentPickup.coin -= coinUse;
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }
    public void GetKey(int keyCount)
    {
        if (keyCount != 0)
        {
            onPlayerPickupDataChanged.RaiseEvent(currentPickup);
            currentPickup.key += keyCount;
        }
        else
        {
            currentPickup.getGoldenKey = true;
            onPlayerGetGoldenKey.RaiseEvent(currentPickup.getGoldenKey);
        }
    }
    public void UseKey(int keyUse)
    {
        currentPickup.key -= keyUse;
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }
    public void GetBomb(int bombCount)
    {
        currentPickup.bomb += bombCount;
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }
    public void UseBomb(int bombUse)
    {
        currentPickup.bomb -= bombUse;
        onPlayerPickupDataChanged.RaiseEvent(currentPickup);
    }
    #endregion

    public void GetDie()
    {
        CustomDebugger.Log("Die");
    }
}
