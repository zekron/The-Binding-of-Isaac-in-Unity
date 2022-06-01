using CustomPhysics2D;
using UnityEngine;

public class Rock : MonoBehaviour, IObjectInRoom, IDestroy
{
	private CustomPlatform2D platform;
	private SpriteRenderer objectRenderer;
	private GameCoordinate coordinate;

	public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }
	public SpriteRenderer ObjectRenderer => objectRenderer;

	private void Awake()
	{
		platform = GetComponent<CustomPlatform2D>();
		objectRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
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

	// Start is called before the first frame update
	void Start()
	{

	}

    public void ResetObject()
    {
       
    }
}
