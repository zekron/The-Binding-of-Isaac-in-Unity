using UnityEngine;

public interface ICollectInSlot
{
    public string ObjectTitle { get; }
    public string ObjectMessage { get; }
    public Sprite SpriteInSlot { get; }

    /// <summary>
    /// 收集到道具栏
    /// </summary>
    public void CollectInSlot();
    /// <summary>
    /// 使用道具栏内道具
    /// </summary>
    public void Activate();
}