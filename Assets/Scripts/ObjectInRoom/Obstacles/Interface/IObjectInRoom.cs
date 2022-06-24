using UnityEngine;

public interface IObjectInRoom
{
	public GameCoordinate Coordinate { get; set; }
	public SpriteRenderer ObjectRenderer { get; }

	public void ChangeRendererOrder();
	public void ResetObject();
}