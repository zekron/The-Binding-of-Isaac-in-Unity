using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;

    [SerializeField] private HealthDataEventChannelSO onPlayerHealthDataChanged;
    [SerializeField] private PickupDataEventChannelSO onPlayerPickupDataChanged;
    [SerializeField] private FloatEventChannelSO onPlayerTearsChanged;
    [SerializeField] private BooleanEventChannelSO onPlayerGetGoldenKey;

    private PlayerProfileTreeElement playerProfile;
    private PlayerRenderer playerRenderer;
    private PlayerController playerController;
    private CustomRigidbody2D playerRigidbody;

    #region Current player status
    private HealthData currentHealth;
    private PickupData currentPickup;

    private float baseDamageUps, damageMultiplier, flatDamageUps = 0;
    private float tearAdjustment, tearAddition, tearMultiplier = 0;
    private float shotSpeedAddition = 0;
    private float moveSpeedAddition = 0;
    private float luckAddition = 0;
    private float rangeAddition = 0;
    /// <summary>
    /// itemPackage[0]二进制最低位为缺省值
    /// </summary>
    private int[] itemPackage;
    private Queue<ItemTreeElement> trinketItemPackage;
    private int trinketPackageMaxCapacity = 1;

    private CollectibleItemTreeElement activeItem;
    private UnityEvent activeItemSkill;
    #endregion

    public int CoinCount => currentPickup.coin;
    public int KeyCount => currentPickup.key;
    public int BombCount => currentPickup.bomb;
    public bool GetGoldenKey => currentPickup.getGoldenKey;
    public Vector2 MoveDirection => playerController.PlayerMoveDirection;
    public float Damage => PlayerProfileTreeElement.GetEffectiveDamage(playerProfile.BaseDamage * DamageMultiplier, baseDamageUps, flatDamageUps);
    public float BaseDamageUps
    {
        get => baseDamageUps;
        set => baseDamageUps = value;
    }

    public float DamageMultiplier
    {
        get => playerProfile.DamageMultiplier * damageMultiplier;
        set => damageMultiplier = value;
    }
    public float FlatDamageUps { get => flatDamageUps; set => flatDamageUps = value; }

    public bool ControllerEnabled { get => playerController.ControllerEnabled; set => playerController.ControllerEnabled = value; }
    public CustomRigidbody2D PlayerRigidbody
    {
        get
        {
            if (playerRigidbody == null)
                playerRigidbody = playerController.CustomRigidbody != null ? playerController.CustomRigidbody : GetComponent<CustomRigidbody2D>();

            return playerRigidbody;
        }
    }

    /// <summary>
    /// 每秒发射眼泪数（眼泪数/秒）
    /// </summary>
    public float Tears
    {
        get => playerProfile.GetTears(TearDelay);
        set
        {
            tearAdjustment = value;
            onPlayerTearsChanged.RaiseEvent(Tears);
        }
    }
    /// <summary>
    /// 两发眼泪间隔（单位：帧数）
    /// </summary>
    public float TearDelay
    {
        get => PlayerProfileTreeElement.GetTearDelay(tearAdjustment, TearAddition, TearMultiplier);
    }
    public float TearAddition
    {
        get => playerProfile.PlayerTearsAddition + tearAddition;
        set => tearAddition = value;
    }
    public float TearMultiplier
    {
        get => playerProfile.PlayerTearsMultiplier + tearMultiplier;
        set => tearMultiplier = value;
    }
    /// <summary>
    /// 眼泪的移动速度
    /// </summary>
    public float ShotSpeed
    {
        get => playerProfile.ShotSpeed + shotSpeedAddition;
        set => shotSpeedAddition = value;
    }
    public float MoveSpeed
    {
        get => playerProfile.BaseMoveSpeed + moveSpeedAddition;
        set => moveSpeedAddition = value;
    }
    public float Luck
    {
        get => playerProfile.Luck + luckAddition;
        set => luckAddition = value;
    }
    public float Range
    {
        get => playerProfile.BaseRange + rangeAddition;
        set => rangeAddition = value;
    }

    private void Awake()
    {
        Initialize((int)playerCharacter);
        currentHealth = playerProfile.PlayerHealthData;
        currentPickup = playerProfile.PlayerPickupData;

        playerRenderer = GetComponent<PlayerRenderer>();
        playerController = GetComponent<PlayerController>();

        activeItemSkill = new UnityEvent();
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

        //int itemCount = GameMgr.Instance.GetCollectibleItemCount() + 1;
        //var length = itemCount / 32 + itemCount % 32 == 0 ? 0 : 1;
        itemPackage = new int[8];
    }

    #region Renderer
    public void SetMoveAnimator(float x, float y)
    {
        playerRenderer.SetMoveAnimator(x, y);
    }
    public void SetHeadSpriteGroup(HeadSpriteGroup spriteGroup)
    {
        playerRenderer.SetHeadSpriteGroup(spriteGroup);
    }
    public void RevertHeadSpriteGroup()
    {
        playerRenderer.RevertHeadSpriteGroup();
    }
    public void SetHeadSprite(Vector2 direction, int index)
    {
        playerRenderer.SetHeadSprite(direction, index);
    }

    internal Vector3 GetTearSpawnPosition(Vector2 tearDirection)
    {
        return playerRenderer.GetTearSpawnPosition(tearDirection);
    }
    #endregion

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
    private bool invincible = false;
    public void GetDamage(int damage)
    {
        if (invincible) return;

        currentHealth -= damage;
        onPlayerHealthDataChanged.RaiseEvent(currentHealth);

        //invincible
        invincible = true;
        playerRenderer.SetInvincibleAnimation();

        if (currentHealth == HealthData.Zero) GetDie();
    }
    internal void OnInvincibleAnimationFinished()
    {
        invincible = false;
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

    #region Items
    public void GetTrinket(ItemTreeElement item)
    {
        trinketItemPackage.Enqueue(item);
        if (trinketItemPackage.Count > trinketPackageMaxCapacity)
        {
            ObjectPoolManager.Release(trinketItemPackage.Dequeue().ItemPrefab, transform.position);
        }
    }

    Dictionary<ModifierType, Modifier> modifierDic = new Dictionary<ModifierType, Modifier>();
    /// <summary>
    /// Register modifier to dictionary and execute modifier.ModifierToRegister()
    /// </summary>
    /// <param name="modifier"></param>
    public void AddSpecificEffect(Modifier modifier)
    {
        if (!modifierDic.ContainsKey(modifier.Type))
        {
            modifierDic.Add(modifier.Type, modifier);
            modifier.ModifierToRegister.Invoke();
        }
#if UNITY_EDITOR
        else
        {
            CustomDebugger.LogWarming($"modifierDic has contained {modifier.Type}");
        }
#endif
    }

    /// <summary>
    /// Execute effectDic[<paramref name="type"/>].ModifierToRemove before remove modifier from player's modifierDictionary
    /// </summary>
    /// <param name="type">registered in dictionary</param>
    public void RemoveSpecificEffect(ModifierType type)
    {
        if (modifierDic.ContainsKey(type))
        {
            modifierDic[type].ModifierToRemove.Invoke();
            modifierDic.Remove(type);
        }
    }

    public void GetPassiveItem(int itemID)
    {
        itemPackage[itemID / 32] = itemID % 32 == 0 ? 0 : 1 << (itemID % 32);
    }

    /// <summary>
    /// 是否有这个道具
    /// </summary>
    /// <param name="types"></param>
    /// <returns></returns>
    public bool CheckPassiveItem(params CollectibleItemName[] types)
    {
        int id;
        for (int i = 0; i < types.Length; i++)
        {
            id = (int)types[i];
            if ((itemPackage[id / 32] & (1 << (id % 32))) == 1) return true;
        }
        return false;
    }
    public CollectibleItemTreeElement GetActiveItem(CollectibleItemTreeElement collectibleItem, UnityAction skill)
    {
        activeItemSkill.RemoveAllListeners();
        activeItemSkill.AddListener(skill);

        var oldItem = activeItem;
        activeItem = collectibleItem;
        return oldItem;
    }
    public void UseActiveItem()
    {
        activeItemSkill.Invoke();
    }
    #endregion

    public void GetDie()
    {
        CustomDebugger.Log("Die");
    }
}
