using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IObjectInRoom
{
	private CustomPlatform2D platform;
	private SpriteRenderer spriteRenderer;
	private GameCoordinate coordinate;

	public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }
	public SpriteRenderer ObjectRenderer { get => spriteRenderer; }

	private void Awake()
	{
		platform = GetComponent<CustomPlatform2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		platform.onTriggerEnter += DestroySelf;
	}

	public void ChangeRendererOrder()
	{
		spriteRenderer.sortingOrder = coordinate.y * -5;
	}

	private void OnDisable()
	{
		platform.onTriggerEnter -= DestroySelf;
	}

	private void DestroySelf(CollisionInfo2D collisionInfo)
	{
		if (collisionInfo.collider.CompareTag(""))
		{

		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}
}
