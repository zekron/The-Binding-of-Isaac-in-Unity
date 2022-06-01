using CustomPhysics2D;
using UnityEngine;

public class TNT : MonoBehaviour, IObjectInRoom, IHealth
{
    [SerializeField] private int maxHealth;
    [SerializeField] private CustomFrameAnimationClip objectClip;

    private GameCoordinate coordinate;
    private CustomPlatform2D platform;
    private SpriteRenderer objectRenderer;
    private int currentHealth;

    public int Health => currentHealth;
    public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }
    public SpriteRenderer ObjectRenderer => objectRenderer;

    void Awake()
    {
        platform = GetComponent<CustomPlatform2D>();
        objectRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        ResetObject();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeRendererOrder()
    {
        objectRenderer.sortingOrder = coordinate.y * -5;
    }

    public void DestroySelf()
    {
        platform.SelfCollider.IsTrigger = true;
    }

    public void GetDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        objectRenderer.sprite = objectClip.NextFrame();
        
        if (currentHealth == 0) DestroySelf();
    }

    public void GetHealing(int healing)
    {
        return;
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + healing);
    }

    public void ResetObject()
    {
        currentHealth = maxHealth;
        objectClip.ResetClip();
        objectRenderer.sprite = objectClip.CurrentFrame();
    }
}
